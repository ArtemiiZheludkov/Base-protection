using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;

namespace Dashboard
{
    public class CurrencyElement : MonoBehaviour
    {
        [HideInInspector] public int Value;

        public string definitionId;
        public TMP_Text balanceField;

        public async Task Init()
        {
            Value = PlayerPrefs.GetInt(definitionId, 0);
            LocalUpdateCurrency();
            
            try
            {
                GetBalancesOptions options = new GetBalancesOptions { ItemsPerFetch = 5 };
                GetBalancesResult getBalancesResult = await EconomyService.Instance.PlayerBalances.GetBalancesAsync(options);
            
                if (getBalancesResult is null) 
                    return;
            
                foreach (var balance in getBalancesResult.Balances)
                {
                    if (string.Equals(balance.CurrencyId, definitionId))
                    {
                        Value = (int)balance.Balance;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            
            LocalUpdateCurrency();
        }

        public void SetCurrency(int value)
        {
            Value = value;
            LocalUpdateCurrency();
        }
        
        public void AddCurrency(int add)
        {
            Value += add;
            LocalUpdateCurrency();
        }

        public void GetCurrency(int get)
        {
            Value -= get;
            LocalUpdateCurrency();
        }

        public async void SaveCurrency()
        {
            PlayerPrefs.SetInt(definitionId, Value);

            try
            {
                await EconomyService.Instance.PlayerBalances.SetBalanceAsync(definitionId, Value);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        
        private void LocalUpdateCurrency()
        {
            balanceField.text = Value.ToString();
            PlayerPrefs.SetInt(definitionId, Value);
        }
        
        private void OnApplicationQuit() => SaveCurrency();
    }
}