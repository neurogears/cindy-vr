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
using System.IO;
using System.Text;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Combinator)]
public class DeviceTCPClient
{
    public int Port { get; set; } 
    private TcpClient client;
    public string IPAddress { get; set; }


    public IObservable<string> Process(IObservable<string> source)
    {

        return Observable.Using(
            () =>
            {
                var client = new TcpClient(IPAddress,Port);
                client.NoDelay = true;
                return client;
            },
            client =>
            {
                var stream = client.GetStream();
                var reader = new StreamReader(stream, Encoding.ASCII);
                var writer = stream;
                //new StreamWriter(stream, Encoding.ASCII) { NewLine = "\n\r" };
                var readAsync = Observable.FromAsync(reader.ReadLineAsync).Repeat();
                return source.Do( s=>
                    {
                        Byte[] data = System.Text.Encoding.ASCII.GetBytes(s+"\n");
                        writer.Write(data,0,data.Length);
                    }).IgnoreElements().Merge(readAsync);
            });
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