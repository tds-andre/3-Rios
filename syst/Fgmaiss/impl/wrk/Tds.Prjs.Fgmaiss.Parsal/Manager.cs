using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Tds.Prjs.Fgmaiss.ParSal.Models;
using System.IO;
using Tds.Prjs.Fgmaiss.ParSal.Crawlers;

namespace Tds.Prjs.Fgmaiss.ParSal
{
   public class Manager
    {
        

        private String AppDataDir;
        public List<CivilServant> Servants = null;
        private List<BotContext> Calls = new List<BotContext>();
        private int Returns;
        private int Sents;
        private int MaxThreads = 3;
        public int Counter = 0;
        public ManualResetEvent TerminateSignal = new ManualResetEvent(false);
        private Semaphore Garg = new Semaphore(15, 15);
        public event EventHandler<BotEventArgs> BotFinished;
        public event EventHandler<BotEventArgs> BotStarted;
        public event EventHandler Terminated;
        public List<CivilServant> GetServantList() {
            var f = File.OpenText("c:\\parsal\\cs.csv");
            String line;
            var models = new List<CivilServant>();
            while ((line = f.ReadLine()) != null)
            {             

                    var model = CivilServant.ParseCsv(line);
                    models.Add(model);                   
               
            }
           return models;
        }
        private void OnTerminated() { if (Terminated != null)Terminated(this, null); }
        private void OnBotFinished(BotContext ctx) {
           
                if (BotFinished != null)
                    BotFinished(this, new BotEventArgs(ctx));
            
        }
        private void OnBotStarted(BotContext ctx) { 
          
                if (BotStarted != null)
                    BotStarted(this, new BotEventArgs(ctx)); 
            
        }

        public Manager(String appDataDir){
           
            AppDataDir = appDataDir;
            
            
        }


        private void BotCall(Object obj)
        {
            Garg.WaitOne();
            BotContext ctx = (BotContext)obj;
            OnBotStarted(ctx);
            ctx.Result = ctx.Bot.Execute(ctx.Servant);            
            ctx.TerminateSignal.Set();
            lock(Garg){
                Counter++;
                if (Counter == Servants.Count)
                    TerminateSignal.Set();
            }
            OnBotFinished(ctx);            
            Garg.Release();
        }

        public void Start(){
           
            Servants = GetServantList();



            Calls.Clear();
            Counter = 0;
            foreach (var servant in Servants)
            {
                var botCtx = new BotContext(new FillServantEarning(), servant);
                Calls.Add(botCtx);
                ThreadPool.QueueUserWorkItem(BotCall, botCtx);
            }

            TerminateSignal.WaitOne();

            
            OnTerminated();             
        }

      

        public struct BotContext{
            public ManualResetEvent TerminateSignal;
            public FillServantEarning Bot;
            public CivilServant Result;
            public Boolean IsFinished;
            public Boolean IsStarted;
            public CivilServant Servant;
           

            public BotContext(FillServantEarning bot, CivilServant servant){
                Bot = bot;
                TerminateSignal = new ManualResetEvent(false);
                Result = null;
                IsFinished = false;
                IsStarted = false;
                Servant = servant;
            }
        }

        public class BotEventArgs: EventArgs{
            public BotContext BotInfo;
            public BotEventArgs(BotContext botInfo)
            {
                BotInfo = botInfo;
                
            }
        }

       
       
    }
}

