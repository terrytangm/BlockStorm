using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockStorm.Utils
{

    public class ValueChangedEventArgs<TK, TV> : EventArgs
    {
        public TK Key { get; set; }
        public TV Value { get; set; }
        public ValueChangedEventArgs(TK key, TV value)
        {
            Key = key;
            Value = value;
        }
    }

    public class ObservableDictionary<TKey, TValue>
    {
        public object objLock = new();

        public Dictionary<TKey, TValue> Dict;
        public event EventHandler<ValueChangedEventArgs<TKey, TValue>>? OnValueChanged;
        public ObservableDictionary(Dictionary<TKey, TValue> dict)
        {
            Dict = dict;
        }
        public TValue this[TKey Key]
        {
            get { return Dict[Key]; }
            set
            {
                lock (objLock)
                {
                    bool changed = false;
                    if (!Dict.ContainsKey(Key))
                    {
                        changed = true;
                    }
                    else 
                    {
                        if (Dict[Key] == null && value != null)
                            changed = true;
                        else if (Dict[Key] != null && !Dict[Key].Equals(value))
                            changed = true;
                    }
                    Dict[Key] = value;
                    try
                    {
                        if (changed && OnValueChanged != null)
                        {
                            OnValueChanged(this, new ValueChangedEventArgs<TKey, TValue>(Key, value));
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }
    }

}
