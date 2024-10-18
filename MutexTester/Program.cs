
UseMutexAndWaitForInput();

void UseMutexAndWaitForInput()
{
    //using var mutex = new Mutex(false, @"Global\MyMutex");
    using var mutex = new Mutex(false, @"Global\MyMutex");
    using var localMutex = new Mutex(false, @"MyMutex.TheSecond");
    
    if (!WaitHandle.WaitAll([mutex, localMutex], 1000))
    {
        Console.WriteLine("Another instance of the app is running. Exiting.");
        return;
    }

    Console.WriteLine("Running the app. Press any key to exit.");
    Console.ReadKey();
}
