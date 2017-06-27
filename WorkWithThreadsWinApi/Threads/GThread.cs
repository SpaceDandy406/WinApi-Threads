using System;
using System.Runtime.InteropServices;

namespace WorkWithThreadsWinApi.Threads
{
	public class GThread
	{
		private bool _flag = true;
		private readonly ThreadDelegate _delegate;

		public ThreadPriority Priority
		{
			get { return WinApiHelper.GetThreadPriority(Handle); }
			set { WinApiHelper.SetThreadPriority(Handle, (int)value); }
		}

		public uint Id { get; private set; }

		public uint Handle { get; private set; }

		public TimeSpan ThreadTime
		{
			get { return DiagnosticHelper.GetThreadTime(Id); }
		}

		public GThread()
		{
			_delegate += CalculatePi;
		}

		public void Start()
		{
			var func = Marshal.GetFunctionPointerForDelegate(_delegate);

			uint lpThreadId;
			Handle = WinApiHelper.CreateThread(IntPtr.Zero, 0, func, IntPtr.Zero, 0, out lpThreadId);
			Id = lpThreadId;
		}

		public GThread(ThreadPriority priority)
			: this()
		{
			Priority = priority;
		}

		public void DestroyGThread()
		{
			_flag = false;
			WinApiHelper.WaitForSingleObject(Handle, 10000);
			WinApiHelper.CloseHandle(Handle);
		}

		private void CalculatePi()
		{
			double i = 0;
			// ReSharper disable once NotAccessedVariable
			double pi = 0;
			while (_flag)
			{
				pi += (1.0 / (1.0 + 2.0 * i)) * ((i % 2 == 0) ? 1 : (-1));
				i++;
			}
		}
	}
}
