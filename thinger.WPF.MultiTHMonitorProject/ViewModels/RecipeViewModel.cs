using ImTools;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using thinger.DataConvertLib;
using thinger.WPF.MultiTHMonitorHelper;
using thinger.WPF.MultiTHMonitorModels.Recipe;
using thinger.WPF.MultiTHMonitorProject.Command;

namespace thinger.WPF.MultiTHMonitorProject.ViewModels
{
    public class RecipeViewModel : BindableBase
    {
        public RecipeViewModel()
        {
            AddRecipeCommand = new DelegateCommand(ExeAddRececipe);
            DeleteRecipeCommand = new DelegateCommand<string>(ExeDelRececipe);
            ApplyRecipeCommand = new DelegateCommand<string>(ExeApplyRececipe);
            SelectRecipeCommand = new DelegateCommand<string>(ExeSelectRecipe);
            ModifyRecipeCommand = new DelegateCommand<string>(ExeModifyRecipe);
            this.CurrentRecipeName = CommonMethods.Device.CurrentRecipe;
            RefreshRecipe();
        }

       
        #region 命令属性
        public DelegateCommand AddRecipeCommand { get; set; }
        public DelegateCommand<string> DeleteRecipeCommand { get; set; }
        public DelegateCommand<string> ApplyRecipeCommand { get; set; }
        public DelegateCommand<string> SelectRecipeCommand { get; set; }
        public DelegateCommand<string> ModifyRecipeCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand ConfirmCommand { get; set; }
        #endregion


        #region 成员变量
        private string BaseRecipe = Environment.CurrentDirectory + "\\Recipe";
        private string devPath = string.Empty;
        #endregion

        #region 属性
        private List<RecipeInfo> recipeInfos;

        public List<RecipeInfo> RecipeInfos
        {
            get { return recipeInfos; }
            set { recipeInfos = value; RaisePropertyChanged(); }
        }
        private string currentRecipeName;

        public string CurrentRecipeName
        {
            get { return currentRecipeName; }
            set { currentRecipeName = value; RaisePropertyChanged(); }
        }


        private string recipeName;

        public string RecipeName
        {
            get { return recipeName; }
            set { recipeName = value; RaisePropertyChanged(); }
        }
        #region 1#站点
        private string stateHTemp01 = "0.0";

        public string StateHTemp01
        {
            get { return stateHTemp01; }
            set { stateHTemp01 = value; RaisePropertyChanged(); }
        }

        private string stateLTemp01 = "0.0";

        public string StateLTemp01
        {
            get { return stateLTemp01; }
            set { stateLTemp01 = value; RaisePropertyChanged(); }
        }


        private string stateHHum01 = "0";

        public string StateHHum01
        {
            get { return stateHHum01; }
            set { stateHHum01 = value; RaisePropertyChanged(); }
        }


        private string stateLHum01 = "0";

        public string StateLHum01
        {
            get { return stateLHum01; }
            set { stateLHum01 = value; RaisePropertyChanged(); }
        }

        private bool isAlarmTemp01;

        public bool IsAlarmTemp01
        {
            get { return isAlarmTemp01; }
            set
            { isAlarmTemp01 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmHum01;

        public bool IsAlarmHum01
        {
            get { return isAlarmHum01; }
            set
            { isAlarmHum01 = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 2#站点
        private string stateHTemp02 = "0.0";

        public string StateHTemp02
        {
            get { return stateHTemp02; }
            set { stateHTemp02 = value; RaisePropertyChanged(); }
        }



        private string stateLTemp02 = "0.0";

        public string StateLTemp02
        {
            get { return stateLTemp02; }
            set { stateLTemp02 = value; RaisePropertyChanged(); }
        }


        private string stateHHum02 = "0";

        public string StateHHum02
        {
            get { return stateHHum02; }
            set { stateHHum02 = value; RaisePropertyChanged(); }
        }


        private string stateLHum02 = "0";

        public string StateLHum02
        {
            get { return stateLHum02; }
            set { stateLHum02 = value; RaisePropertyChanged(); }
        }

        private bool isAlarmTemp02;

        public bool IsAlarmTemp02
        {
            get { return isAlarmTemp02; }
            set
            { isAlarmTemp02 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmHum02;

        public bool IsAlarmHum02
        {
            get { return isAlarmHum02; }
            set
            { isAlarmHum02 = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 3#站点
        private string stateHTemp03 = "0.0";

        public string StateHTemp03
        {
            get { return stateHTemp03; }
            set { stateHTemp03 = value; RaisePropertyChanged(); }
        }



        private string stateLTemp03 = "0.0";

        public string StateLTemp03
        {
            get { return stateLTemp03; }
            set { stateLTemp03 = value; RaisePropertyChanged(); }
        }


        private string stateHHum03 = "0";

        public string StateHHum03
        {
            get { return stateHHum03; }
            set { stateHHum03 = value; RaisePropertyChanged(); }
        }


        private string stateLHum03 = "0";

        public string StateLHum03
        {
            get { return stateLHum03; }
            set { stateLHum03 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmTemp03;

        public bool IsAlarmTemp03
        {
            get { return isAlarmTemp03; }
            set
            { isAlarmTemp03 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmHum03;

        public bool IsAlarmHum03
        {
            get { return isAlarmHum03; }
            set
            { isAlarmHum03 = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 4#站点
        private string stateHTemp04 = "0.0";

        public string StateHTemp04
        {
            get { return stateHTemp04; }
            set { stateHTemp04 = value; RaisePropertyChanged(); }
        }


        private string stateLTemp04 = "0.0";

        public string StateLTemp04
        {
            get { return stateLTemp04; }
            set { stateLTemp04 = value; RaisePropertyChanged(); }
        }


        private string stateHHum04 = "0";

        public string StateHHum04
        {
            get { return stateHHum04; }
            set { stateHHum04 = value; RaisePropertyChanged(); }
        }


        private string stateLHum04 = "0";

        public string StateLHum04
        {
            get { return stateLHum04; }
            set { stateLHum04 = value; RaisePropertyChanged(); }
        }

        private bool isAlarmTemp04;

        public bool IsAlarmTemp04
        {
            get { return isAlarmTemp04; }
            set
            { isAlarmTemp04 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmHum04;

        public bool IsAlarmHum04
        {
            get { return isAlarmHum04; }
            set
            { isAlarmHum04 = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 5#站点
        private string stateHTemp05 = "0.0";

        public string StateHTemp05
        {
            get { return stateHTemp05; }
            set { stateHTemp05 = value; RaisePropertyChanged(); }
        }


        private string stateLTemp05 = "0.0";

        public string StateLTemp05
        {
            get { return stateLTemp05; }
            set { stateLTemp05 = value; RaisePropertyChanged(); }
        }


        private string stateHHum05 = "0";

        public string StateHHum05
        {
            get { return stateHHum05; }
            set { stateHHum05 = value; RaisePropertyChanged(); }
        }


        private string stateLHum05 = "0";
        public string StateLHum05
        {
            get { return stateLHum05; }
            set { stateLHum05 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmTemp05;

        public bool IsAlarmTemp05
        {
            get { return isAlarmTemp05; }
            set
            { isAlarmTemp05 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmHum05;

        public bool IsAlarmHum05
        {
            get { return isAlarmHum05; }
            set
            { isAlarmHum05 = value; RaisePropertyChanged(); }
        }

        #endregion

        #region 6#站点
        private string stateHTemp06 = "0.0";

        public string StateHTemp06
        {
            get { return stateHTemp06; }
            set { stateHTemp06 = value; RaisePropertyChanged(); }
        }



        private string stateLTemp06 = "0.0";

        public string StateLTemp06
        {
            get { return stateLTemp06; }
            set { stateLTemp06 = value; RaisePropertyChanged(); }
        }


        private string stateHHum06 = "0";

        public string StateHHum06
        {
            get { return stateHHum06; }
            set { stateHHum06 = value; RaisePropertyChanged(); }
        }


        private string stateLHum06 = "0";
        public string StateLHum06
        {
            get { return stateLHum06; }
            set { stateLHum06 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmTemp06;

        public bool IsAlarmTemp06
        {
            get { return isAlarmTemp06; }
            set
            { isAlarmTemp06 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmHum06;

        public event Action<IDialogResult> RequestClose;

        public bool IsAlarmHum06
        {
            get { return isAlarmHum06; }
            set
            { isAlarmHum06 = value; RaisePropertyChanged(); }
        }

        public string Title => throw new NotImplementedException();

        #endregion
        #endregion

        #region 方法

        private void ExeAddRececipe()
        {
            RecipeInfo recipeInfo = new RecipeInfo()
            {
                RecipeName=this.RecipeName,
                RecipeParams=new List<RecipeParam>()
                {
                    new RecipeParam(){TempHigh=Convert.ToSingle(StateHTemp01),TempLow=Convert.ToSingle(StateLTemp01),HumidityHigh=Convert.ToSingle(StateHHum01),HumidityLow=Convert.ToSingle(StateHHum01),TempAlarmEnable=IsAlarmTemp01,HumidityAlarmEnable=IsAlarmHum01},
                new RecipeParam(){TempHigh=Convert.ToSingle(StateHTemp02),TempLow=Convert.ToSingle(StateLTemp02),HumidityHigh=Convert.ToSingle(StateHHum02),HumidityLow=Convert.ToSingle(StateHHum02),TempAlarmEnable=IsAlarmTemp02,HumidityAlarmEnable=IsAlarmHum02},
                 new RecipeParam(){TempHigh=Convert.ToSingle(StateHTemp03),TempLow=Convert.ToSingle(StateLTemp03),HumidityHigh=Convert.ToSingle(StateHHum03),HumidityLow=Convert.ToSingle(StateHHum03),TempAlarmEnable=IsAlarmTemp03,HumidityAlarmEnable=IsAlarmHum03},
                  new RecipeParam(){TempHigh=Convert.ToSingle(StateHTemp04),TempLow=Convert.ToSingle(StateLTemp04),HumidityHigh=Convert.ToSingle(StateHHum04),HumidityLow=Convert.ToSingle(StateHHum04),TempAlarmEnable=IsAlarmTemp04,HumidityAlarmEnable=IsAlarmHum04},
                   new RecipeParam(){TempHigh=Convert.ToSingle(StateHTemp05),TempLow=Convert.ToSingle(StateLTemp05),HumidityHigh=Convert.ToSingle(StateHHum05),HumidityLow=Convert.ToSingle(StateHHum05),TempAlarmEnable=IsAlarmTemp05,HumidityAlarmEnable=IsAlarmHum05},
                    new RecipeParam(){TempHigh=Convert.ToSingle(StateHTemp06),TempLow=Convert.ToSingle(StateLTemp06),HumidityHigh=Convert.ToSingle(StateHHum06),HumidityLow=Convert.ToSingle(StateHHum06),TempAlarmEnable=IsAlarmTemp06,HumidityAlarmEnable=IsAlarmHum06}
                }
            };
          var result=  AddRecipe(recipeInfo);
            if (result)
            {
                RefreshRecipe();
            }
        }
        
        private void ExeDelRececipe(string obj)
        {
           var result= DeleteRecipe(obj);
            if (result)
            {
                RefreshRecipe();
            }
        }
        private void ExeSelectRecipe(string obj)
        {
            RecipeInfo recipeInfo = recipeInfos.Where(c => c.RecipeName == obj).FirstOrDefault();
            this.CurrentRecipeName = obj;
            if (recipeInfo == null)
            {
                MessageBox.Show("当前配方信息为空，请检查重新加载配方");
                return;
            }

            this.StateHTemp01 = recipeInfo.RecipeParams[0].TempHigh.ToString();
            this.StateLTemp01 = recipeInfo.RecipeParams[0].TempLow.ToString();
            this.StateHHum01 = recipeInfo.RecipeParams[0].HumidityHigh.ToString();
            this.StateLHum01 = recipeInfo.RecipeParams[0].HumidityLow.ToString();
            this.IsAlarmTemp01 = recipeInfo.RecipeParams[0].TempAlarmEnable;
            this.IsAlarmHum01 = recipeInfo.RecipeParams[0].HumidityAlarmEnable;

            this.StateHTemp02 = recipeInfo.RecipeParams[1].TempHigh.ToString();
            this.StateLTemp02 = recipeInfo.RecipeParams[1].TempLow.ToString();
            this.StateHHum02 = recipeInfo.RecipeParams[1].HumidityHigh.ToString();
            this.StateLHum02 = recipeInfo.RecipeParams[1].HumidityLow.ToString();
            this.IsAlarmTemp02 = recipeInfo.RecipeParams[1].TempAlarmEnable;
            this.IsAlarmHum02 = recipeInfo.RecipeParams[1].HumidityAlarmEnable;

            this.StateHTemp03 = recipeInfo.RecipeParams[2].TempHigh.ToString();
            this.StateLTemp03 = recipeInfo.RecipeParams[2].TempLow.ToString();
            this.StateHHum03 = recipeInfo.RecipeParams[2].HumidityHigh.ToString();
            this.StateLHum03 = recipeInfo.RecipeParams[2].HumidityLow.ToString();
            this.IsAlarmTemp03 = recipeInfo.RecipeParams[2].TempAlarmEnable;
            this.IsAlarmHum03 = recipeInfo.RecipeParams[2].HumidityAlarmEnable;

            this.StateHTemp04 = recipeInfo.RecipeParams[3].TempHigh.ToString();
            this.StateLTemp04 = recipeInfo.RecipeParams[3].TempLow.ToString();
            this.StateHHum04 = recipeInfo.RecipeParams[3].HumidityHigh.ToString();
            this.StateLHum04 = recipeInfo.RecipeParams[3].HumidityLow.ToString();
            this.IsAlarmTemp04 = recipeInfo.RecipeParams[3].TempAlarmEnable;
            this.IsAlarmHum04 = recipeInfo.RecipeParams[3].HumidityAlarmEnable;

            this.StateHTemp05 = recipeInfo.RecipeParams[4].TempHigh.ToString();
            this.StateLTemp05 = recipeInfo.RecipeParams[4].TempLow.ToString();
            this.StateHHum05 = recipeInfo.RecipeParams[4].HumidityHigh.ToString();
            this.StateLHum05 = recipeInfo.RecipeParams[4].HumidityLow.ToString();
            this.IsAlarmTemp05 = recipeInfo.RecipeParams[4].TempAlarmEnable;
            this.IsAlarmHum05 = recipeInfo.RecipeParams[4].HumidityAlarmEnable;

            this.StateHTemp06 = recipeInfo.RecipeParams[5].TempHigh.ToString();
            this.StateLTemp06 = recipeInfo.RecipeParams[5].TempLow.ToString();
            this.StateHHum06 = recipeInfo.RecipeParams[5].HumidityHigh.ToString();
            this.StateLHum06 = recipeInfo.RecipeParams[5].HumidityLow.ToString();
            this.IsAlarmTemp06 = recipeInfo.RecipeParams[5].TempAlarmEnable;
            this.IsAlarmHum06 = recipeInfo.RecipeParams[5].HumidityAlarmEnable;
        }
        private void ExeApplyRececipe(string obj)
        {
            RecipeInfo recipeInfo = recipeInfos.Where(c => c.RecipeName == obj).FirstOrDefault();
            this.CurrentRecipeName = obj;
            if (recipeInfo == null)
            {
                MessageBox.Show("当前配方信息为空，请检查重新加载配方");
                return;
            }
            List<short> values = new List<short>();

            for (int i = 0; i < 6; i++)
            {
                values.Add(Convert.ToInt16(recipeInfo.RecipeParams[i].TempHigh * 10));
                values.Add(Convert.ToInt16(recipeInfo.RecipeParams[i].TempLow * 10));
                values.Add(Convert.ToInt16(recipeInfo.RecipeParams[i].HumidityHigh * 10));
                values.Add(Convert.ToInt16(recipeInfo.RecipeParams[i].HumidityLow * 10));
            }
            for (int i = 0; i < 24; i++)
            {
                values.Add(0);
            }
            for (int i = 0; i < 6; i++)
            {
                values.Add(Convert.ToInt16(recipeInfo.RecipeParams[i].TempAlarmEnable ? 1 : 0));
                values.Add(Convert.ToInt16(recipeInfo.RecipeParams[i].HumidityAlarmEnable ? 1 : 0));
            }

            bool result = CommonMethods.Modbus.PreSetMultiRegisters(36, ByteArrayLib.GetByteArrayFromShortArray(values.ToArray()));

            if (result)
            {
                IniConfigHelper.WriteIniData("设备参数", "当前配方", obj, devPath);
                CurrentRecipeName =obj;
                CommonMethods.Device.CurrentRecipe = obj;
                MessageBox.Show("配方加载成功!");
            }
            else
            {
                MessageBox.Show("配方加载失败!");
            }
        }
        private void ExeModifyRecipe(string obj)
        {
            var info = recipeInfos.Where(c => c.RecipeName == obj).FirstOrDefault();

            if (info == null)
            {
                MessageBox.Show("当前配方名称不存在，请检查");
                return;
            }

            var recipe = GetRecipeInfo();

            if (recipe != null)
            {
                if (AddRecipe(recipe))
                {
                    MessageBox.Show("配方保存成功！");
                    RefreshRecipe();
                }
                else
                {
                    MessageBox.Show("配方保存失败！");
                }
            }
            else
            {
                MessageBox.Show("配方保存失败！");
            }
        }

        /// <summary>
        /// 更新配方
        /// </summary>
        private void RefreshRecipe()
        {
            RecipeInfos = GetAllRecipe();

            if (RecipeInfos.Count > 0)
            {
               //RecipeInfos.
                //SetRecipeInfo(recipeInfos[index]);
            }
        }

        /// <summary>
        /// 获取配方
        /// </summary>
        /// <returns></returns>
        private RecipeInfo GetRecipeInfo()
        {
            RecipeInfo recipeInfo = new RecipeInfo();
            recipeInfo.RecipeName = RecipeName;
            recipeInfo.RecipeParams = new List<RecipeParam>()
            {
                new RecipeParam(){TempHigh=Convert.ToSingle(StateHTemp01),TempLow=Convert.ToSingle(StateLTemp01),HumidityHigh=Convert.ToSingle(StateHHum01),HumidityLow=Convert.ToSingle(StateHHum01),TempAlarmEnable=IsAlarmTemp01,HumidityAlarmEnable=IsAlarmHum01},
                new RecipeParam(){TempHigh=Convert.ToSingle(StateHTemp02),TempLow=Convert.ToSingle(StateLTemp02),HumidityHigh=Convert.ToSingle(StateHHum02),HumidityLow=Convert.ToSingle(StateHHum02),TempAlarmEnable=IsAlarmTemp02,HumidityAlarmEnable=IsAlarmHum02},
                 new RecipeParam(){TempHigh=Convert.ToSingle(StateHTemp03),TempLow=Convert.ToSingle(StateLTemp03),HumidityHigh=Convert.ToSingle(StateHHum03),HumidityLow=Convert.ToSingle(StateHHum03),TempAlarmEnable=IsAlarmTemp03,HumidityAlarmEnable=IsAlarmHum03},
                  new RecipeParam(){TempHigh=Convert.ToSingle(StateHTemp04),TempLow=Convert.ToSingle(StateLTemp04),HumidityHigh=Convert.ToSingle(StateHHum04),HumidityLow=Convert.ToSingle(StateHHum04),TempAlarmEnable=IsAlarmTemp04,HumidityAlarmEnable=IsAlarmHum04},
                   new RecipeParam(){TempHigh=Convert.ToSingle(StateHTemp05),TempLow=Convert.ToSingle(StateLTemp05),HumidityHigh=Convert.ToSingle(StateHHum05),HumidityLow=Convert.ToSingle(StateHHum05),TempAlarmEnable=IsAlarmTemp05,HumidityAlarmEnable=IsAlarmHum05},
                    new RecipeParam(){TempHigh=Convert.ToSingle(StateHTemp06),TempLow=Convert.ToSingle(StateLTemp06),HumidityHigh=Convert.ToSingle(StateHHum06),HumidityLow=Convert.ToSingle(StateHHum06),TempAlarmEnable=IsAlarmTemp06,HumidityAlarmEnable=IsAlarmHum06}
            };
            return recipeInfo;
        }
        /// <summary>
        /// 设置配方
        /// </summary>
        /// <param name="recipeInfo"></param>
        private void SetRecipeInfo(RecipeInfo recipeInfo)
        {
            if (recipeInfo.RecipeParams.Count == 6)
            {
               RecipeName = recipeInfo.RecipeName;
                if (recipeInfo.RecipeParams[0]!=null)
                {
                    StateHTemp01 = recipeInfo.RecipeParams[0].TempHigh.ToString();
                    StateHHum01 = recipeInfo.RecipeParams[0].HumidityHigh.ToString();
                    StateLTemp01 = recipeInfo.RecipeParams[0].TempLow.ToString();
                    StateLHum01 = recipeInfo.RecipeParams[0].HumidityLow.ToString();
                    IsAlarmTemp01 = recipeInfo.RecipeParams[0].TempAlarmEnable;
                    IsAlarmHum01 = recipeInfo.RecipeParams[0].HumidityAlarmEnable;
                }
                if (recipeInfo.RecipeParams[1] != null)
                {
                    StateHTemp02 = recipeInfo.RecipeParams[1].TempHigh.ToString();
                    StateHHum02 = recipeInfo.RecipeParams[1].HumidityHigh.ToString();
                    StateLTemp02 = recipeInfo.RecipeParams[1].TempLow.ToString();
                    StateLHum02 = recipeInfo.RecipeParams[1].HumidityLow.ToString();
                    IsAlarmTemp02 = recipeInfo.RecipeParams[1].TempAlarmEnable;
                    IsAlarmHum02 = recipeInfo.RecipeParams[1].HumidityAlarmEnable;
                }
                if (recipeInfo.RecipeParams[2] != null)
                {
                    StateHTemp03 = recipeInfo.RecipeParams[2].TempHigh.ToString();
                    StateHHum03 = recipeInfo.RecipeParams[2].HumidityHigh.ToString();
                    StateLTemp03 = recipeInfo.RecipeParams[2].TempLow.ToString();
                    StateLHum03 = recipeInfo.RecipeParams[2].HumidityLow.ToString();
                    IsAlarmTemp03 = recipeInfo.RecipeParams[2].TempAlarmEnable;
                    IsAlarmHum03 = recipeInfo.RecipeParams[2].HumidityAlarmEnable;
                }
                if (recipeInfo.RecipeParams[3] != null)
                {
                    StateHTemp04 = recipeInfo.RecipeParams[3].TempHigh.ToString();
                    StateHHum04 = recipeInfo.RecipeParams[3].HumidityHigh.ToString();
                    StateLTemp04 = recipeInfo.RecipeParams[3].TempLow.ToString();
                    StateLHum04 = recipeInfo.RecipeParams[3].HumidityLow.ToString();
                    IsAlarmTemp04 = recipeInfo.RecipeParams[3].TempAlarmEnable;
                    IsAlarmHum04 = recipeInfo.RecipeParams[3].HumidityAlarmEnable;
                }
                if (recipeInfo.RecipeParams[4] != null)
                {
                    StateHTemp05 = recipeInfo.RecipeParams[4].TempHigh.ToString();
                    StateHHum05 = recipeInfo.RecipeParams[4].HumidityHigh.ToString();
                    StateLTemp05 = recipeInfo.RecipeParams[4].TempLow.ToString();
                    StateLHum05 = recipeInfo.RecipeParams[4].HumidityLow.ToString();
                    IsAlarmTemp05 = recipeInfo.RecipeParams[4].TempAlarmEnable;
                    IsAlarmHum05= recipeInfo.RecipeParams[4].HumidityAlarmEnable;
                }
                if (recipeInfo.RecipeParams[5] != null)
                {
                    StateHTemp06 = recipeInfo.RecipeParams[6].TempHigh.ToString();
                    StateHHum06 = recipeInfo.RecipeParams[6].HumidityHigh.ToString();
                    StateLTemp06 = recipeInfo.RecipeParams[6].TempLow.ToString();
                    StateLHum06 = recipeInfo.RecipeParams[6].HumidityLow.ToString();
                    IsAlarmTemp06 = recipeInfo.RecipeParams[6].TempAlarmEnable;
                    IsAlarmHum06 = recipeInfo.RecipeParams[6].HumidityAlarmEnable;
                }
            }
        }

        /// <summary>
        /// 添加配方
        /// </summary>
        /// <param name="recipeInfo"></param>
        /// <returns></returns>
        private bool AddRecipe(RecipeInfo recipeInfo)
        {
            return IniConfigHelper.WriteIniData("配方", "配方数据", JSONHelper.EntityToJSON(recipeInfo), BaseRecipe + "\\" + recipeInfo.RecipeName + ".ini");
        }
        /// <summary>
        /// 获取所有配方
        /// </summary>
        /// <returns></returns>
        private List<RecipeInfo> GetAllRecipe()
        {
            List<FileInfo> folder = new DirectoryInfo(BaseRecipe).GetFiles("*.ini").ToList();

            List<RecipeInfo> recipeInfos = new List<RecipeInfo>();

            foreach (var item in folder)
            {
                recipeInfos.Add(GetRecipe(item.FullName));
            }
            return recipeInfos;
        }
        /// <summary>
        /// 获取配方
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private RecipeInfo GetRecipe(string path)
        {
            return JSONHelper.JSONToEntity<RecipeInfo>(IniConfigHelper.ReadIniData("配方", "配方数据", "", path));
        }
        /// <summary>
        /// 根据配方名字删除配方
        /// </summary>
        /// <param name="recipeName"></param>
        /// <returns></returns>
        private bool DeleteRecipe(string recipeName)
        {
            try
            {
                File.Delete(BaseRecipe + "\\" + recipeName + ".ini");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        
        #endregion
    }
}
