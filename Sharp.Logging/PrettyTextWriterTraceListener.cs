// Copyright (C) 2018 (to be determined)

using System;
using System.Diagnostics;
using System.IO;

namespace Sharp.Logging
{
    /// <summary>
    ///   Custom trace listener that writes to a log file in a concise format.
    /// </summary>
    public class PrettyTextWriterTraceListener : TextWriterTraceListener
    {
        private const string
            DefaultLogFileName = "Log.txt";

        public PrettyTextWriterTraceListener()
            : base(Console.Out) { }

        public PrettyTextWriterTraceListener(string fileName)
            : base(GetFullPath(fileName)) { }

        public override void TraceEvent(TraceEventCache e, string source, TraceEventType type, int id)
        {
            TraceEvent(e, source, type, id, string.Empty);
        }

        public override void TraceEvent(TraceEventCache e, string source, TraceEventType type, int id, string message)
        {
            if (!ShouldTrace(e, source, type, id, message))
                return;

            WriteHeader(e, type, id);
            Writer.Write(message);
            WriteFooter(source);
        }

        public override void TraceEvent(TraceEventCache e, string source, TraceEventType type, int id, string format, params object[] args)
        {
            if (!ShouldTrace(e, source, type, id, format, args))
                return;

            WriteHeader(e, type, id);
            if (args != null)
                Writer.Write(string.Format(format, args));
            else
                Writer.Write(format);
            WriteFooter(source);
        }

        public override void TraceData(TraceEventCache e, string source, TraceEventType type, int id, object obj)
        {
            if (!ShouldTrace(e, source, type, id, obj: obj))
                return;

            WriteHeader(e, type, id);
            Writer.Write(Format(obj));
            WriteFooter(source);
        }

        public override void TraceData(TraceEventCache e, string source, TraceEventType type, int id, params object[] objs)
        {
            if (!ShouldTrace(e, source, type, id, objs: objs))
                return;

            WriteHeader(e, type, id);
            WriteObjects(objs);
            WriteFooter(source);
        }

        public override void TraceTransfer(TraceEventCache e, string source, int id, string message, Guid relatedActivityId)
        {
            TraceEvent(e, source, TraceEventType.Transfer, id, "{0} {related:{1}}", message, relatedActivityId.ToString());
        }

        public override void Write(string message)
        {
            Writer.Write(message);
            NeedIndent = false;
        }

        public override void WriteLine(string message)
        {
            Writer.WriteLine(message);
            NeedIndent = true;
        }

        protected void WriteHeader(TraceEventCache e, TraceEventType type, int id)
        {
            var writer = Writer;

            if (!NeedIndent)
                // Events always start on a new line.
                writer.WriteLine();

            // Date, Time
            writer.Write('[');
            writer.Write(e.DateTime.ToString("yyyy-MM-dd HH:mm:ss.fff", writer.FormatProvider));
            writer.Write(']');
            writer.Write(' ');

            // Process, Thread
            writer.Write('(');
            writer.Write(e.ProcessId);
            writer.Write(':');
            writer.Write(e.ThreadId);
            writer.Write(')');
            writer.Write(' ');

            // Correlation Id
            var operations = e.LogicalOperationStack;
            if (operations.Count > 0)
            {
                writer.Write('{');
                writer.Write(operations.Peek());
                writer.Write('}');
                writer.Write(' ');
            }

            // Event Type
            writer.Write(type.ToString());
            writer.Write(':');
            writer.Write(' ');

            // Event ID
            if (id != 0)
            {
                writer.Write(' ');
                writer.Write('#');
                writer.Write(id);
                writer.Write(':');
                writer.Write(' ');
            }

            NeedIndent = false;
        }

        private void WriteFooter(string source)
        {
            var writer = Writer;

            // Source
            writer.Write(' ');
            writer.Write('<');
            writer.Write(source);
            writer.Write('>');

            // EOL
            writer.WriteLine();
            NeedIndent = true;
        }

        private void WriteObjects(object[] objs)
        {
            var writer = Writer;

            if (objs != null && objs.Length != 0)
            {
                writer.Write(Format(objs[0]));

                for (var i = 1; i < objs.Length; i++)
                {
                    writer.Write(", ");
                    writer.Write(Format(objs[i]));
                }
            }

            NeedIndent = false;
        }

        protected sealed override void WriteIndent()
        {
            // Do nothing
        }

        private static string Format(object obj)
        {
            return obj == null ? "(null)" : obj.ToString();
        }

        private bool ShouldTrace(TraceEventCache e, string source, TraceEventType type, int id,
            string message = null, object[] args = null, object obj = null, object[] objs = null)
        {
            var filter = Filter;
            return filter == null
                || filter.ShouldTrace(e, source, type, id, message, args, obj, objs);
        }

        private static string GetFullPath(string fileName)
        {
            // Choose a reasonable default file name
            var path = string.IsNullOrEmpty(fileName)
                ? DefaultLogFileName
                : fileName;

            try
            {
                // Convert a relative path to full
                if (!Path.IsPathRooted(path))
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

                // Ensure the directory exists and is writable
                EnsureDirectory(path);

                // The path *should* work at this point
                return path;
            }
            catch (Exception)
            {
                // Use best effort to give the admin a helpful hint
                NotifyCannotCreateLogFile(path);
                throw;
            }
        }

        private static void EnsureDirectory(string path)
        {
            // Ensure the directory exists
            var directory = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);

            // Probe for writability by creating a random file
            var probe = Path.Combine(directory, Path.GetRandomFileName());
            using (File.Open(probe, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read)) { }
            File.Delete(probe);
        }

        private static void NotifyCannotCreateLogFile(string path)
        {
            try
            {
                using (var log = new EventLog("Application"))
                {
                    log.Source = "Application";
                    log.WriteEntry(
                        $"Unable to create log file: {path}",
                        EventLogEntryType.Error
                    );
                }
            }
            catch
            {
                // This code is best-effort only and should not overwrite
                // whatever exception is being processed already.
            }
        }
    }
}
