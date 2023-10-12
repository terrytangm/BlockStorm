using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlockStorm.Utils
{
    public enum ChainConfigName
    {
        ETHMainNetLocal,
        ETHMainNetInfura,
        ETHMainNetAlchemy,
        ETHMainNetQuickNode,
        ETHGoerliAlchemy,
        ArbitrumQuickNode,
        BSCQuickNode,
        ArbitrumAlchemy,
        ArbitrumInfura,
        ETHGoerliFork,
        BSCFork,
        ETHMainNetFork
    }

    public enum ChainConfigPart
    {
        ChainID,
        HttpURL,
        WebsocketURL
    }

    public class Config
    {
        private static readonly string configfile = Directory.GetCurrentDirectory() + "/BlockStorm.Utils.dll.config";
        private static Configuration config = GetConfig(configfile);

        public static Configuration GetConfig(string filename)
        {
            var fileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = filename
            };
            config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            return config;
        }

        public static string GetConnectionString(string dbName)
        {
            return config.ConnectionStrings.ConnectionStrings[dbName].ConnectionString;
        }

        public static string GetValueByKey(string key)
        {
            string? value = null;
            if (config.AppSettings is not null)
            {
                value = config.AppSettings.Settings[key].Value;
            }
            return value;
        }

        public static string GetValueByKey(string chainID, string key)
        {
            string keyName = chainID + "-" + key;
            string? value = null;
            if (config.AppSettings is not null)
            {
                value = config.AppSettings.Settings[keyName].Value;
            }
            return value;
        }

        public static string GetWrappedNativeAddress(string chainID)
        {
            return GetValueByKey(chainID, "WrappedNative");
        }

        public static string GetControllerAddress(string chainID)
        {
            return GetValueByKey(chainID, "Controller");
        }

        public static string GetRelayerAddress(string chainID)
        {
            return GetValueByKey(chainID, "Relayer");
        }
        public static string GetPinkLock02Address(string chainID)
        {
            return GetValueByKey(chainID, "PinkLock02");
        }

        public static string? GetControllerOwnerPK(string? chainID)
        {
            return Crypto.RST_AesDecrypt_Base64(GetValueByKey(chainID, "ControllerOwner"));
        }
        public static string GetUniswapV2RouterAddress(string chainID)
        {
            return GetValueByKey(chainID, "UniswapV2Router");
        }

        public static string GetUniV2FactoryAddress(string chainID)
        {
            return GetValueByKey(chainID, "FactoryAddress");
        }

        public static string GetUniV2FactoryCodeHash(string chainID)
        {
            return GetValueByKey(chainID, "FactoryCodeHash");
        }

        public static string ConfigInfo(ChainConfigName? _chainConfigName, ChainConfigPart part)
        {
            string? chainConfigName = null;
            if (_chainConfigName == null)
            {
                if (config.AppSettings is not null)
                {
                    chainConfigName = config.AppSettings.Settings["TargetChainConfig"].Value;
                }
            }
            else
            {
                chainConfigName = _chainConfigName.Value.ToString();
            }
            if (string.IsNullOrEmpty(chainConfigName))
            {
                throw new Exception("TargetChainConfig does not exists in config");
            }

            var section = config.GetSection("ChainConfigSection") as ChainConfigSelectionElement ?? throw new Exception("ChainConfigSection not found");
            var chainConfig  = section.Configs.Get(chainConfigName);
            if (part == ChainConfigPart.ChainID) return chainConfig.ChainID.ToString();
            if (part == ChainConfigPart.HttpURL) return chainConfig.HttpURL;
            if (part == ChainConfigPart.WebsocketURL) return chainConfig.WebsocketURL;
            return string.Empty;
        }

        internal static string? GetAesKey()
        {
            string? value = null;
            if (config.AppSettings is not null)
            {
                value = config.AppSettings.Settings["AesKey"].Value;
            }
            return value;
        }


    }

    public class ChainConfig : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["Name"]; }
        }
        [ConfigurationProperty("ChainID", IsRequired = true)]
        public int ChainID
        {
            get { return (int)this["ChainID"]; }
        }
        [ConfigurationProperty("HttpURL", IsRequired = true)]
        public string HttpURL
        {
            get { return (string)this["HttpURL"]; }
        }
        [ConfigurationProperty("WebsocketURL", IsRequired = true)]
        public string WebsocketURL
        {
            get { return (string)this["WebsocketURL"]; }
        }
    }

    public class ChainConfigCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 重载生成ConfigurationElement 节点方法
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ChainConfig();
        }
        /// <summary>
        /// 将ConfigurationElement 转换成自定义的对象
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element is not ChainConfig config)
                throw new ArgumentNullException("element is not MachineElement");

            return config.Name;
        }
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        /// <summary>
        /// ConfigurationElement节点对应的名称
        /// </summary>
        protected override string ElementName
        {
            get
            {
                return "ChainConfig";
            }
        }

        /// <summary>
        /// 根据唯一标示的名称获取自定义ConfigurationElement节点对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ChainConfig Get(string name)
        {
            return (ChainConfig)BaseGet(name);
        }
    }

    /// <summary>
    /// 自定义Section节点对象
    /// </summary>
    public class ChainConfigSelectionElement : ConfigurationSection
    {
        /// <summary>
        /// Section 节点集合对应的名称
        /// </summary>
        [ConfigurationProperty("ChainConfigs")]
        public ChainConfigCollection Configs
        {
            get { return (ChainConfigCollection)this["ChainConfigs"]; }
        }
    }
}
