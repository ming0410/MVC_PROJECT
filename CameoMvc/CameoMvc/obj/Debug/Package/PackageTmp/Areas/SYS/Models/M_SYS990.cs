using CameoMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CameoMvc.Areas.SYS.Models
{
    public class M_SYS990
    {
        public string FUNC { get; } = "SYS990";
        public string FUNC_NAME { get; } = "我的系統";

        #region 基本參數
        public bool FLAG { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ERR { get; set; }

        /// <summary>
        /// 是否為手機，非平板
        /// </summary>
        public bool PHON { get; set; }

        /// <summary>
        /// 是否為行動裝置，手機及平板
        /// </summary>
        public bool MOBL { get; set; }

        /// <summary>
        /// 是否為測試環境
        /// </summary>
        public bool ENV_T { get; set; }

        /// <summary>
        /// 廠別ID
        /// </summary>
        public string FAB_ID { get; set; }

        /// <summary>
        /// 使用者ID
        /// </summary>
        public string USER_ID { get; set; }

        /// <summary>
        /// 使用者工號
        /// </summary>
        public string USER_NO { get; set; }

        /// <summary>
        /// 使用者姓名
        /// </summary>
        public string USER_NAME { get; set; }

        /// <summary>
        /// 使用者權限
        /// </summary>
        public List<Authority> AUTHORITY { get; set; }
        #endregion

        /// <summary>
        /// 使用者代理程式
        /// </summary>
        public string USER_ANGET { get; set; } = string.Empty;

        /// <summary>
        /// 瀏覽器
        /// </summary>
        public string BROWSER { get; set; } = string.Empty;

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ERR_MSG { get; set; } = string.Empty;
    }
}