using SyncDataJob.Utility;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTA.PI.Models;
using GTA.PI.Logics;
using MongoDB.Driver;
using System.Collections.Generic;

namespace SyncDataJob
{
    public partial class frmMain : Form
    {
        #region 变量属性

        /// <summary>
        /// 上次开始同步时间
        /// </summary>
        private DateTime PreStartTime;

        /// <summary>
        /// 上次完成同步时间
        /// </summary>
        private DateTime PreFinishedTime;

        /// <summary>
        /// 取得当前同步状态
        /// </summary>
        /// <returns></returns>
        private EnmSyncDataStatus SyncDataStatus
        {
            get
            {
                if (this.PreFinishedTime < this.PreStartTime)
                {
                    return EnmSyncDataStatus.Running;
                }

                if (this.PreFinishedTime < DateTime.Today)
                {
                    return EnmSyncDataStatus.Waiting;
                }

                return EnmSyncDataStatus.Finished;
            }
        }

        /// <summary>
        /// 同步数据状态
        /// </summary>
        private enum EnmSyncDataStatus
        {
            Waiting,
            Running,
            Finished
        }

        #endregion

        public frmMain()
        {
            InitializeComponent();

            this.Init();
            this.dtpSync.Value = new DateTime(this.dtpSync.Value.Year, this.dtpSync.Value.Month, this.dtpSync.Value.Day, 2, 0, 0);
            if (this.ConnectDSP() == false)
            {
                MessageBox.Show("连接DSP失败，请联系管理员！");
                this.Close();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            if (MongoDBHelper.Count<SymbolInfo>() > 0)
            {
                this.PreFinishedTime = this.PreStartTime = DateTime.Now;
            }
            else
            {
                this.PreFinishedTime = this.PreStartTime = new DateTime(1990, 1, 1);

                string indexSecurityIDAsc = "{SecurityID:1}";
                MongoDBHelper.CreateOneIndexAsync<SymbolInfo>(indexSecurityIDAsc);
                MongoDBHelper.CreateOneIndexAsync<BOLLResult>(indexSecurityIDAsc);
                MongoDBHelper.CreateOneIndexAsync<DMIResult>(indexSecurityIDAsc);
                MongoDBHelper.CreateOneIndexAsync<KDJResult>(indexSecurityIDAsc);
                MongoDBHelper.CreateOneIndexAsync<L1QuoteInfo>(indexSecurityIDAsc);
                MongoDBHelper.CreateOneIndexAsync<MACDResult>(indexSecurityIDAsc);
                MongoDBHelper.CreateOneIndexAsync<MAResult>(indexSecurityIDAsc);
                MongoDBHelper.CreateOneIndexAsync<RSIResult>(indexSecurityIDAsc);
                MongoDBHelper.CreateOneIndexAsync<WRResult>(indexSecurityIDAsc);

                string indexSecurityIDAsc_TradingDateAsc = "{SecurityID:1,TradingDate:1}";
                MongoDBHelper.CreateOneIndexAsync<DMIHistory>(indexSecurityIDAsc_TradingDateAsc);
                MongoDBHelper.CreateOneIndexAsync<KDJHistory>(indexSecurityIDAsc_TradingDateAsc);
                MongoDBHelper.CreateOneIndexAsync<MACDHistory>(indexSecurityIDAsc_TradingDateAsc);

                string indexPlateIDAsc = "{PlateID:1}";
                MongoDBHelper.CreateOneIndexAsync<PlateInfo>(indexPlateIDAsc);
                MongoDBHelper.CreateOneIndexAsync<PlateSymbolInfo>(indexPlateIDAsc);

                string indexCodeAsc_SelectTermAsc = "{Code:1,SelectTerm:1}";
                MongoDBHelper.CreateOneIndexAsync<IndexDetailDTO>(indexCodeAsc_SelectTermAsc);

                string indexTypeCodeAsc = "{TypeCode:1}";
                MongoDBHelper.CreateOneIndexAsync<IndexTreeDTO>(indexTypeCodeAsc);

                string indexSecurityIDAsc_TradingDateDesc = "{SecurityID:1,TradingDate:-1}";
                List<CreateIndexModel<DataByTime>> lstIndexDataByTime = new List<CreateIndexModel<DataByTime>>();
                lstIndexDataByTime.Add(new CreateIndexModel<DataByTime>(indexSecurityIDAsc_TradingDateAsc));
                lstIndexDataByTime.Add(new CreateIndexModel<DataByTime>(indexSecurityIDAsc_TradingDateDesc));
                MongoDBHelper.CreatManyIndexAsync<DataByTime>(lstIndexDataByTime);

                string indexSecurityIDAsc_TradingDateNameAsc = "{SecurityID:1,TradingDateName:1}";
                string indexIndexCodeAsc_TradingDateNameDesc = "{IndexCode:1,TradingDateName:-1}";
                string indexIndexCodeAsc_TradingDateDesc = "{IndexCode:1,TradingDate:-1}";
                List<CreateIndexModel<DataByTimeIndexInfo>> lstIndexDataByTimeIndexInfo = new List<CreateIndexModel<DataByTimeIndexInfo>>();
                lstIndexDataByTimeIndexInfo.Add(new CreateIndexModel<DataByTimeIndexInfo>(indexSecurityIDAsc_TradingDateNameAsc));
                lstIndexDataByTimeIndexInfo.Add(new CreateIndexModel<DataByTimeIndexInfo>(indexIndexCodeAsc_TradingDateNameDesc));
                lstIndexDataByTimeIndexInfo.Add(new CreateIndexModel<DataByTimeIndexInfo>(indexIndexCodeAsc_TradingDateDesc));
                MongoDBHelper.CreatManyIndexAsync<DataByTimeIndexInfo>(lstIndexDataByTimeIndexInfo);

                string indexSecurityIDAsc_EndDateNameDesc = "{SecurityID:1,EndDateName:-1}";
                string indexSecurityIDAsc_EndDateDesc = "{SecurityID:1,EndDate:-1}";
                string indexIndexCodeAsc_EndDateNameDesc = "{IndexCode:1,EndDateName:-1}";
                List<CreateIndexModel<FinanceIndexInfo>> lstIndexFinanceIndexInfo = new List<CreateIndexModel<FinanceIndexInfo>>();
                lstIndexFinanceIndexInfo.Add(new CreateIndexModel<FinanceIndexInfo>(indexSecurityIDAsc_EndDateNameDesc));
                lstIndexFinanceIndexInfo.Add(new CreateIndexModel<FinanceIndexInfo>(indexSecurityIDAsc_EndDateDesc));
                lstIndexFinanceIndexInfo.Add(new CreateIndexModel<FinanceIndexInfo>(indexIndexCodeAsc_EndDateNameDesc));
                MongoDBHelper.CreatManyIndexAsync<FinanceIndexInfo>(lstIndexFinanceIndexInfo);
            }

            timerJob.Enabled = true;
        }

        /// <summary>
        /// 连接DSP网关
        /// </summary>
        private bool ConnectDSP()
        {
            try
            {
                // 连接DSP网关                
                return DSPHelper.Connect();
            }
            catch (Exception ex)
            {
                // 记日志
                LogHelper.Error("异常", "连接DSP网关失败", ex);
                return false;
            }
        }

        /// <summary>
        /// 安全输出同步状态
        /// </summary>
        /// <param name="content"></param>
        private void PrintSyncStatus(string content)
        {
            content = string.Format("{0}：{1}", DateTime.Now, content);
            this.rtbResult.Invoke(new Action(() => { this.rtbResult.Text = this.rtbResult.Text + content + Environment.NewLine; }));
        }

        /// <summary>
        /// 安全设置按钮可用状态
        /// </summary>
        private void EnableButton(bool enable)
        {
            this.Invoke(new Action(() =>
            {
                this.btnSubmit.Enabled = enable;
                this.btnBasic.Enabled = enable;
                this.btnDataByTime.Enabled = enable;
                this.btnFinance.Enabled = enable;
                this.btnPlateSymbol.Enabled = enable;
                this.btnTechnical.Enabled = enable;
            }));
        }

        #region 启动线程

        /// <summary>
        /// 子线程 同步所有信息
        /// </summary>
        private void SyncAllInTrhead()
        {
            if (this.SyncDataStatus == EnmSyncDataStatus.Running)
            {
                MessageBox.Show("同步操作执行中。");
                return;
            }
            this.rtbResult.Text = "";
            this.EnableButton(false);

            Thread threadSyncData = new Thread(SyncAll);
            threadSyncData.IsBackground = true;
            threadSyncData.Priority = ThreadPriority.Highest;
            threadSyncData.Start();
        }

        /// <summary>
        /// 子线程 同步行情指标
        /// </summary>
        private void SyncDataByTimeInTask()
        {
            this.rtbResult.Text = "";
            this.EnableButton(false);

            TaskAwaiter taskAwaiter = Task.Factory.StartNew(SyncDataByTime).GetAwaiter();
            taskAwaiter.OnCompleted(() => { this.EnableButton(true); });
        }

        /// <summary>
        /// 子线程 同步财务指标
        /// </summary>
        private void SyncFinanceInTask()
        {
            this.rtbResult.Text = "";
            this.EnableButton(false);

            TaskAwaiter taskAwaiter = Task.Factory.StartNew(SyncFinance).GetAwaiter();
            taskAwaiter.OnCompleted(() => { this.EnableButton(true); });
        }

        /// <summary>
        /// 子线程 同步基础数据
        /// </summary>
        private void SyncBasicInTask()
        {
            this.rtbResult.Text = "";
            this.EnableButton(false);

            TaskAwaiter taskAwaiter = Task.Factory.StartNew(SyncBasic).GetAwaiter();
            taskAwaiter.OnCompleted(() => { this.EnableButton(true); });
        }

        /// <summary>
        /// 子线程 同步板块指标
        /// </summary>
        private void SyncPlateSymbolInTask()
        {
            this.rtbResult.Text = "";
            this.EnableButton(false);

            TaskAwaiter taskAwaiter = Task.Factory.StartNew(SyncPlateSymbol).GetAwaiter();
            taskAwaiter.OnCompleted(() => { this.EnableButton(true); });
        }

        /// <summary>
        /// 子线程 同步技术指标
        /// </summary>
        private void SyncTechnicalInTask()
        {
            this.rtbResult.Text = "";
            this.EnableButton(false);

            TaskAwaiter taskAwaiter = Task.Factory.StartNew(SyncTechnical).GetAwaiter();
            taskAwaiter.OnCompleted(() => { this.EnableButton(true); });
        }

        #endregion

        #region 同步

        /// <summary>
        /// 同步所有信息
        /// </summary>
        private void SyncAll()
        {
            LogHelper.Info("总运行", "开始：同步数据");

            this.PreStartTime = DateTime.Now;
            this.PrintSyncStatus("开始：同步数据");
            try
            {
                SyncBasic();
                SyncPlateSymbol();
                SyncDataByTime();
                SyncTechnical();
                SyncFinance();
            }
            catch (Exception ex)
            {
                LogHelper.Error("异常", "同步数据异常.", ex);
            }
            finally
            {
                this.PreFinishedTime = DateTime.Now;
                this.PrintSyncStatus("结束：同步数据");
                LogHelper.Info("总运行", "结束：同步数据");

                this.EnableButton(true);
            }
        }

        /// <summary>
        /// 同步基础信息
        /// </summary>
        private void SyncBasic()
        {
            this.PrintSyncStatus("开始：同步基础信息");

            // 同步所有a股代码数据
            this.PrintSyncStatus("  同步a股代码");
            DspToMongoHelper.SyncA_PlateIndex();

            // 同步交易日历
            this.PrintSyncStatus("  同步交易日历");
            DspToMongoHelper.SyncTradeCalendarIndex();

            // 同步所有板块
            this.PrintSyncStatus("  同步板块信息");
            DspToMongoHelper.SyncPlatesIndex();

            //缓存指标菜单
            this.PrintSyncStatus("  生成指标菜单");
            MongodbCacheHelper.CacheIndexMenu();

            this.PrintSyncStatus("结束：同步基础信息");
        }

        /// <summary>
        /// 同步技术指标
        /// </summary>
        private void SyncTechnical()
        {
            this.PrintSyncStatus("开始：同步技术指标");

            //同步所有的的日线行情数据
            this.PrintSyncStatus("  同步所有日线行情数据");
            DspToMongoHelper.SyncDataByTime();

            this.PrintSyncStatus("  计算技术指标");
            this.CalculateAllTechnical();

            //缓存技术指标的明细
            this.PrintSyncStatus("  生成技术指标分布值");
            MongodbCacheHelper.CacheTechnicalIndex();

            this.PrintSyncStatus("结束：同步技术指标");
        }

        /// <summary>
        /// 计算所有技术指标
        /// </summary>
        public void CalculateAllTechnical()
        {
            this.PrintSyncStatus("  计算MACD");
            CalcIndexHelper.CalculateMACD();

            this.PrintSyncStatus("  计算MA");
            CalcIndexHelper.CalculateMA();

            this.PrintSyncStatus("  计算KDJ");
            CalcIndexHelper.CalculateKDJ();

            this.PrintSyncStatus("  计算BOLL");
            CalcIndexHelper.CalculateBOLL();

            this.PrintSyncStatus("  计算WR");
            CalcIndexHelper.CalculateWR();

            this.PrintSyncStatus("  计算DMI");
            CalcIndexHelper.CalculateDMI();

            this.PrintSyncStatus("  计算RSI");
            CalcIndexHelper.CalculateRSI();
        }

        /// <summary>
        /// 同步行情指标
        /// </summary>
        private void SyncDataByTime()
        {
            this.PrintSyncStatus("开始：同步行情指标");

            //同步前5个交易日的日线行情数据
            this.PrintSyncStatus("  同步前5个交易日的日线行情");
            DspToMongoHelper.SyncDataByTimeIndexInfo();

            //生成行情指标对应的选股条件
            this.PrintSyncStatus("  生成行情指标分布值");
            MongodbCacheHelper.CacheDataByTimeIndex();

            this.EnableButton(true);

            this.PrintSyncStatus("结束：同步行情指标");
        }

        /// <summary>
        /// 同步财务指标
        /// </summary>
        private void SyncFinance()
        {
            this.PrintSyncStatus("开始：同步财务指标");

            // 同步财务指标
            this.PrintSyncStatus("  同步财务指标");
            DspToMongoHelper.SyncFinaceIndex();

            //生成财务指标对应的选股条件
            this.PrintSyncStatus("  生成财务指标分布值");
            MongodbCacheHelper.CacheFinanceIndex();

            this.EnableButton(true);

            this.PrintSyncStatus("结束：同步财务指标");
        }

        /// <summary>
        /// 缓存板块指标
        /// </summary>
        private void SyncPlateSymbol()
        {
            this.PrintSyncStatus("开始：同步板块指标");

            // 缓存板块指标
            this.PrintSyncStatus("  同步板块指标");
            DspToMongoHelper.CachePlatesSymbol();

            //生成板块指标对应的选股条件
            this.PrintSyncStatus("  生成板块指标明细");
            MongodbCacheHelper.CachePlateIndex();

            this.PrintSyncStatus("结束：同步板块指标");
        }

        /// <summary>
        /// 同步最新行情
        /// </summary>
        private void SyncQuickData()
        {
            while (true)
            {
                Thread.Sleep(3000);

                //只在早上9点到晚上6点时间段内同步快照数据
                if (DateTime.Now.Hour >= 9 && DateTime.Now.Hour <= 18)
                {
                    DspToMongoHelper.SyncQuickData();
                }
            }
        }

        #endregion

        #region 事件

        /// <summary>
        /// 发生同步异常时，由管理员手动进行同步
        /// </summary>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            this.SyncAllInTrhead();
        }

        /// <summary>
        /// 基础信息按钮按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBasic_Click(object sender, EventArgs e)
        {
            this.SyncBasicInTask();
        }

        /// <summary>
        /// 板块指标按钮按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlateSymbol_Click(object sender, EventArgs e)
        {
            this.SyncPlateSymbolInTask();
        }

        /// <summary>
        /// 行情指标按钮按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDataByTime_Click(object sender, EventArgs e)
        {
            this.SyncDataByTimeInTask();
        }

        /// <summary>
        /// 财务指标按钮按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFinance_Click(object sender, EventArgs e)
        {
            this.SyncFinanceInTask();
        }

        /// <summary>
        /// 技术指标按钮按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTechnical_Click(object sender, EventArgs e)
        {
            this.SyncTechnicalInTask();
        }

        /// <summary>
        /// 设置每晚冷晨进行同步，直到同步完成结束为止
        /// </summary>
        private void timerJob_Tick(object sender, EventArgs e)
        {
            if (this.SyncDataStatus == EnmSyncDataStatus.Waiting &&
                DateTime.Now.ToString("HH:mm:ss").CompareTo(dtpSync.Value.ToString("HH:mm:ss")) >= 0)
            {
                timerJob.Enabled = false;
                this.SyncAllInTrhead();
                timerJob.Enabled = true;
            }
        }

        /// <summary>
        /// 窗体关闭前
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.SyncDataStatus == EnmSyncDataStatus.Running || this.btnSubmit.Enabled == false)
            {
                if (MessageBox.Show("数据同步中，确定要关闭程序吗？", "确认", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// 画面显示事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Shown(object sender, EventArgs e)
        {
            Task.Factory.StartNew(SyncQuickData, TaskCreationOptions.LongRunning);
        }

        #endregion
    }
}