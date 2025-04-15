namespace core.Common.Exceptions
{
    public static class CustomException
    {
        #region Constants
        public static string LOGDIRECTORYPATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TMdrive-Visualizer", "Log");
        // Path.Combine(Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path)), "Log"); // Get ETW log directory path
        public static string LOGFILEPATH = @"\TMdrive-Visualizer_Log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log"; // Get / Create ETW log file
        private static readonly object lockObj = new object();
        #endregion
        #region Methods
        /// <summary>
        /// Logs Exception Infomation
        /// </summary>
        /// <param name="a_CustomMessage">Custom Message to be logged</param>
        /// <param name="a_Exception">Exception object</param>
        public static void LogExceptionInfo(string a_CustomMessage, Exception a_Exception)
        {
            string? customErrorMessage = null;
            string? systemErrorMessage;

            if (!string.IsNullOrEmpty(a_CustomMessage))
            {
                customErrorMessage = "Error Message:\t" + a_CustomMessage;
                Trace.WriteLine(customErrorMessage);
            }

            systemErrorMessage = "System Message:\t" + a_Exception.Message;
            Trace.WriteLine(systemErrorMessage);

            //Get a StackTrace object for the exception
            StackTrace st = new(a_Exception, true);
            StackFrame? frame;

            if (st.FrameCount > 1)
            {
                //Display the highest-level function call in the trace
                frame = st.GetFrame(st.FrameCount - 1);
            }
            else
            {
                //Get the first stack frame
                frame = st.GetFrame(0);
            }

            //Logs Call Stack
            LogCallStackInfo();

            //log in a file 
            FileLog(customErrorMessage, systemErrorMessage, frame);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_CustomMessage"></param>
        /// <param name="a_MessageType"></param>
        public static void LogCustomMessage(string a_CustomMessage)
        {
            string logFile = LOGDIRECTORYPATH + LOGFILEPATH;
            string messageText = a_CustomMessage;

            if (!Directory.Exists(LOGDIRECTORYPATH))
            {
                Directory.CreateDirectory(LOGDIRECTORYPATH);
            }

            if (!File.Exists(logFile))
            {
                FileStream fs = File.Create(logFile);
                fs.Close();
            }

            lock (lockObj)
            // Create log file 
            {
                using (StreamWriter txtWriter = File.AppendText(logFile))
                {
                    StringBuilder logMessageString = new StringBuilder();
                    logMessageString.Append("[");
                    logMessageString.Append(System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                    logMessageString.Append("]  ");
                    logMessageString.Append(messageText);
                    txtWriter.WriteLine(logMessageString);
                }
            }
        }
        /// <summary>
        /// Log the Exception in a File
        /// </summary>
        /// <param name="a_CustomMessage"></param>
        /// <param name="frame"></param>
        /// <param name="Type"></param>
        private static void FileLog(string? a_CustomMessage, string? a_SystemMessage, StackFrame? frame)
        {
            //Get the file name
            string? fileName = frame?.GetFileName();
            Trace.WriteLine("File Name: " + fileName);

            //Get the method name
            string? methodName = frame?.GetMethod()?.Name;
            Trace.WriteLine("Method Name: " + methodName);

            //Get the line number from the stack frame
            int? line = frame?.GetFileLineNumber();
            Trace.WriteLine("Line Number: " + line?.ToString(CultureInfo.InvariantCulture));

            //Get the column number
            int? col = frame?.GetFileColumnNumber();
            Trace.WriteLine("Column Number: " + col?.ToString(CultureInfo.InvariantCulture));

            if (!Directory.Exists(LOGDIRECTORYPATH))
                Directory.CreateDirectory(LOGDIRECTORYPATH);

            string? logFile = null;
            logFile = LOGDIRECTORYPATH + LOGFILEPATH;

            if (!File.Exists(logFile))
            {
                FileStream fs = File.Create(logFile);
                fs.Close();
            }

            lock (lockObj)
            {
                // Create log file 
                using (StreamWriter txtWriter = File.AppendText(logFile))
                {
                    txtWriter.WriteLine("*************************************************************");
                    txtWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                    txtWriter.WriteLine(a_CustomMessage);
                    txtWriter.WriteLine(a_SystemMessage);
                    txtWriter.WriteLine("File Name:\t" + fileName);
                    txtWriter.WriteLine("Method Name:\t" + methodName);
                    txtWriter.WriteLine("Line Number:\t" + line?.ToString(CultureInfo.InvariantCulture));
                    txtWriter.WriteLine("*************************************************************");
                }
            }
        }
        /// <summary>
        /// Logs Call Stack Information
        /// </summary>
        public static void LogCallStackInfo()
        {
            try
            {
                StackTrace callStack = new StackTrace(true);
                Trace.WriteLine("Call Stack:\n");
                for (int i = 2; i < callStack?.FrameCount; i++)
                {
                    if (callStack?.GetFrame(i)?.GetFileLineNumber() != 0)
                    {
                        Trace.WriteLine(callStack?.GetFrame(i)?.GetFileName() + " " + callStack?.GetFrame(i)?.GetMethod() + " " + callStack?.GetFrame(i)?.GetFileLineNumber());
                    }
                }
            }
            catch (Exception a_Exception)
            {
                Trace.WriteLine(a_Exception);
            }
        }
        #endregion Methods
    }
}