using DotNet_HeapAndStackViewer;
using Microsoft.Diagnostics.Runtime;
using System.Diagnostics;

while (true)
{
    var processId = GetProcessIdOrDefault();
    if (processId == null)
        return;

    var dataTarget = DataTarget.CreateSnapshotAndAttach(processId.Value);

    foreach (var clr in dataTarget.ClrVersions)
    {
        Console.WriteLine($"CLR {clr.Version}");

        var runtime = clr.CreateRuntime();
        foreach (var thread in runtime.Threads)
        {
            Console.WriteLine($"    Thread {thread.ManagedThreadId}");

            foreach (var stack in thread.EnumerateStackTrace())
            {
                Console.WriteLine($"    Stack {stack.StackPointer}{Environment.NewLine}" +
                    $"      Instruction Pointer: {stack.InstructionPointer}{Environment.NewLine}" +
                    $"      Kind: {stack.Kind}{Environment.NewLine}" +
                    $"      Method: {stack.Method}{Environment.NewLine}" +
                    $"      Frame: {stack.FrameName}{Environment.NewLine}" +
                    $"      Context: {stack.Context.ToString()}");
            }
        }

        Console.WriteLine($"    Heap:");
        foreach (var heap in runtime.Heap.EnumerateObjects())
        {
            Console.WriteLine($"    Object:{Environment.NewLine}" +
                $"      Type: {heap.Type}{Environment.NewLine}" +
                $"      Size: {heap.Size}{Environment.NewLine}" +
                $"      Address: {heap.Address}{Environment.NewLine}" +
                $"      IsNull: {heap.IsNull}{Environment.NewLine}");
        }
    }
}

static int? GetProcessIdOrDefault()
{
    var processId = ConsoleReader.GetText("Process Id: ");

    if (string.IsNullOrWhiteSpace(processId))
    {
        const string yesOption = "y";
        const string noOption = "n";

        var option = ConsoleReader.GetOption(
            "If a process ID is not provided, the current process will be used for experiment. Do you want to continue (y/n)?",
            [yesOption, noOption]);

        return option == yesOption
            ? Process.GetCurrentProcess().Id
            : null;
    }

    if (int.TryParse(processId, out int result)) 
        return result;

    return GetProcessIdOrDefault();
}