using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using TMPro;

namespace Interlude
{
    public class ScavengerHunt : MonoBehaviour
    {
        private static ScavengerHunt instance;

        public string serverIP = "35.193.152.147:8080";

        public TextMeshProUGUI addressInput;
        string address;
        string ticket;
        string key;

        public UnityEvent OnConnectionError;
        public UnityEvent OnCheckSuccess;
        public UnityEvent OnNullTicket;

        /***********************
        * Interface
        ************************/

        public static void KeyFound(int id, string password)
        {
            instance.StartCoroutine(instance.FetchKeyAndDisplay(id, password));
        }

        public static void PromptAddress()
        {
            instance.ShowAddressPromptWindow();
        }

        /***********************
        * GameObject
        ************************/
        private void Start()
        {
            instance = this;
        }

        public void CheckPlayerTicket()
        {
            StartCoroutine(CheckPlayerTicketRoutine());
        }

        IEnumerator CheckPlayerTicketRoutine()
        {
            address = CleanString(addressInput.text);

            WWWForm form = new WWWForm();
            form.AddField("player", address);
            UnityWebRequest www = UnityWebRequest.Post(GetURL("ticket"), form);
            www.timeout = 20;
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(www.error);
                OnConnectionError.Invoke();
            }
            else
            {
                ticket = CleanString(www.downloadHandler.text);

                if (ticket == "0x0000000000000000000000000000000000000000000000000000000000000000")
                {
                    OnNullTicket.Invoke();
                }
                else
                {
                    OnCheckSuccess.Invoke();
                }
            }
        }

        IEnumerator FetchKeyAndDisplay(int id, string password)
        {
            //int hashedPassword = HashPassword(password);
            WWWForm form = new WWWForm();
            form.AddField("ticket", ticket);
            form.AddField("player", address);
            form.AddField("id", id);
            form.AddField("password", password);
            UnityWebRequest www = UnityWebRequest.Post(GetURL("getKey"), form);
            www.timeout = 20;

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                key = www.downloadHandler.text.Substring(1, www.downloadHandler.text.Length-2);
                DisplayKey(id);
            }
        }

        /***********************
        * util
        ************************/
        string CleanString(string s)
        {
            return s.Replace("\u200B", "").Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\"", "");
        }

        string GetURL(string args)
        {
            return "http://" + serverIP + "/" + args;
        }

        int HashPassword(string password)
        {
            return GetStableHash(password + address);//use address as salt
        }

        int GetStableHash(string s)
        {
            uint hash = 0;
            var bytes = System.Text.Encoding.ASCII.GetBytes(s);
            foreach (byte b in bytes)
            {
                hash += b;
                hash += (hash << 10);
                hash ^= (hash >> 6);
            }
            // final avalanche
            hash += (hash << 3);
            hash ^= (hash >> 11);
            hash += (hash << 15);

            return (int)(hash);
        }

        public void CopyKeyToClipboard()
        {
            GUIUtility.systemCopyBuffer = instance.key;
        }

        /***********************
        * UI
        ************************/
        public GameObject canvas;
        public Animator addressPromptAnimator, keyFoundAnimator;
        public TextMeshProUGUI keyText, keyIdText;
        void ShowAddressPromptWindow()
        {
            instance.canvas.SetActive(true);
            addressPromptAnimator.CrossFade("Menu In", 0);
        }

        void ShowKeyFoundWindow()
        {
            canvas.SetActive(true);
            keyFoundAnimator.CrossFade("Menu In", 0);
        }

        public void CloseWindow(Animator animator)
        {
            animator.CrossFade("Menu Out", 0);
            StartCoroutine(DeactivateCanvas());
        }

        IEnumerator DeactivateCanvas()
        {
            yield return new WaitForSeconds(1);
            canvas.SetActive(false);
        }

        void DisplayKey(int id)
        {
            Debug.Log(key);
            keyText.text = key.ToString();
            keyIdText.text = id.ToString();
            ShowKeyFoundWindow();
        }
    }
}

