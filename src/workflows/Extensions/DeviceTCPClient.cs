using Bonsai;
using System;
using System.Net.Sockets;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Reactive.Linq;
using System.Reactive.Disposables;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Combinator)]
public class DeviceTCPClient
{
    public int Port { get; set; } 
    private TcpClient client;
    public string IPAddress { get; set; }
    private NetworkStream stream;

    public IObservable<string> Process(IObservable<string> source)
    {
        client = new TcpClient(IPAddress,Port);
        client.NoDelay = true;
        stream = client.GetStream();
        //string latestValue ="testeCaralho";
        //source.Select(value => {latestValue=value;return latestValue;});
        return Observable.Create<string>((observer, cancellationToken) =>
        {
            
            source.Subscribe(
                        Write);
                        //,
                        //observer.OnError,
                        //observer.OnCompleted);
            return Task.Factory.StartNew(() =>
                    {
                        ManualResetEvent mre = new ManualResetEvent(false);
                        Byte[] dataRec = new Byte[256];
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            String responseData = String.Empty;
                            Int32 bytes = stream.Read(dataRec, 0, dataRec.Length);
                            responseData = System.Text.Encoding.ASCII.GetString(dataRec, 0, bytes);
                            mre.WaitOne(200);
                            observer.OnNext(responseData);
                        }
                    },
                    cancellationToken,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);
        }).PublishReconnectable().RefCount();
    }
    private void Write(string msg)
    {
        //msg="SET BANK ODOR Bank8 3\n";
        Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg+'\n');          
        stream.Write(data, 0, data.Length);
    }
}

/*
   public IObservable<int> Process()
    {
    
        return Observable.Create<int>((observer, cancellationToken)=>
        {
            return Task.Factory.StartNew(() =>
                {
                    int count =0;
                  
                    long timeCouter = Milis;
                    busyCrono.Start();
                    while(!cancellationToken.IsCancellationRequested)
                    {
                        var time =  timeCouter -busyCrono.ElapsedMilliseconds;
                        if(time > 0)
                        {
                            mre.WaitOne((int)time);
                            
                        }
                            
                        timeCouter += Milis;     
                        observer.OnNext(count);
                        count++;           
                    }
                },
                cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }).PublishReconnectable().RefCount();
    }
*/
