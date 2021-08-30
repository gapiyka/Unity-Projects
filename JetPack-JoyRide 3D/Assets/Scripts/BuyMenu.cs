using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class BuyMenu : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Button button1;
    //[SerializeField] private Button jetbut1;
    //[SerializeField] private Button jetbut2;
    //[SerializeField] private Button jetbut3;
    //[SerializeField] private Button jetbut4;
    //[SerializeField] private Button jetbut5;
    [SerializeField] private Button[] JetBut;
    [SerializeField] private Button vehbut1;
    [SerializeField] private Button vehbut2;
    [SerializeField] private Button CostumesButton;
    [SerializeField] private Button JetpacksButton;
    [SerializeField] private Button VehiclesButton;
    [SerializeField] private Button GadgetsButton;
    [SerializeField] private GameObject MyItems;
    [SerializeField] private GameObject ForSale;
    [SerializeField] private GameObject MyItemsJet;
    [SerializeField] private GameObject ForSaleJet;
    [SerializeField] private GameObject MyItemsVeh;
    [SerializeField] private GameObject ForSaleVeh;
    [SerializeField] private GameObject CostumesMenu;
    [SerializeField] private GameObject JetpacksMenu;
    [SerializeField] private GameObject VehiclesMenu;
    [SerializeField] private GameObject GadgetsMenu;
    [SerializeField] private GameObject BG;
    [SerializeField] private GameObject HideObj1;
    [SerializeField] private GameObject HideObj2;
    [SerializeField] private GameObject CoinsField;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Costume1;
    [SerializeField] private GameObject Costume2;
    [SerializeField] private GameObject[] Jets;
    private int coins;
    private Button[] boughtItem = new Button[2];
    private Button[] boughtItemJet = new Button[5];
    private Button[] boughtItemVeh = new Button[2];
    private string path;
    private string[] items;
    public int[] vehicles;
    Dictionary<string, Button> dict = new Dictionary<string, Button>();
    [SerializeField] private PLControl2 playerController;
    [SerializeField] private CanvasScript canv;

    const int DIF = 50;

    void Start()
    {
        path = Application.dataPath + "/Items.txt";
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "button\njetbut1\n\n");
        }
        else
        {
            items = File.ReadAllLines(path);
            Spliter();
        }
        vehicles = new int[2] { 0, 0 };

        Button CB = CostumesButton.GetComponent<Button>();
        CB.onClick.AddListener(TaskOnCostumes);
        Button JB = JetpacksButton.GetComponent<Button>();
        JB.onClick.AddListener(TaskOnJetPacks);
        Button VB = VehiclesButton.GetComponent<Button>();
        VB.onClick.AddListener(TaskOnVehicles);
        Button GB = GadgetsButton.GetComponent<Button>();
        GB.onClick.AddListener(TaskOnGadgets);
        ReCoins();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CostumesMenu.gameObject.SetActive(false);
            JetpacksMenu.gameObject.SetActive(false);
            VehiclesMenu.gameObject.SetActive(false);
            GadgetsMenu.gameObject.SetActive(false);
            BG.gameObject.SetActive(false);
            HideObj1.gameObject.SetActive(true);
            HideObj2.gameObject.SetActive(true);
        }
    }

    void BoughtCostume() 
    {
        boughtItem[1] = button1;
        HidePriceUI(button1);
        button1.GetComponent<RectTransform>().localPosition += transform.up * (DIF-10);
        ForSale.GetComponent<RectTransform>().localPosition += transform.up * -DIF;

        button1.onClick.RemoveAllListeners();
        button1.onClick.AddListener(() => ChooseSkin(Costume2));
    }
    void TaskOnClickBuy()//costumes
    {
        int price = Convert.ToInt32(button1.transform.GetChild(1).gameObject.GetComponent<Text>().text);
        if (price <= Coins())
        {
            BoughtCostume();

            MinusCoins(price);
            ReCoins();
            Saver();
        }
    }
    void RemoveAnySkin()
    {
        string[] SkinNames = new string[] { "Armature", "Armature(Clone)", "Skin2(Clone)"};
        foreach (string name in SkinNames)
        {
            var findSkin = Player.transform.Find(name);
            if (findSkin != null) Destroy(findSkin.gameObject);
        }

    }
    void ChooseSkin(GameObject Costume)
    {
        RemoveAnySkin();
        Instantiate(Costume, Player.transform);
    }


    void BoughtJet(int index, Button button)
    {
        const int LAST_INDEX = 4;

        boughtItemJet[index] = button;
        HidePriceUI(button);
        button.GetComponent<RectTransform>().localPosition += transform.up * (DIF-10);
        ForSaleJet.GetComponent<RectTransform>().localPosition += transform.up * -DIF;
        if (index != LAST_INDEX)
        {
            Button jbtn = JetBut[index+1];
            jbtn.onClick.AddListener(() => TaskOnClickBuyJet(index+1, jbtn));
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => ChooseJet(index));
    }
    void TaskOnClickBuyJet(int index, Button button)//jetpacks
    {
        
        int price = Convert.ToInt32(button.transform.GetChild(1).gameObject.GetComponent<Text>().text);
        if (price <= Coins())
        {
            BoughtJet(index, button);

            MinusCoins(price);
            ReCoins();
            Saver();
        }
    }
    void RemoveAnyJet()
    {
        if (Player.transform.Find("JetPack") != null) Destroy(Player.transform.Find("JetPack").gameObject);
        else
        {
            for(int i = 1; i <= 5; i++)
            {
                var findJet = Player.transform.Find($"Jet{i}(Clone)");
                if (findJet != null) Destroy(findJet.gameObject);
            }
        }
    }
    void ChooseJet(int index)
    {
        RemoveAnyJet();
        Instantiate(Jets[index], Player.transform);
        Debug.Log(index);
        Debug.Log(Jets[index]);
        Debug.Log(JetBut[index]);
    }


    void BoughtVehicle(int index, Button button)
    {
        boughtItemVeh[index] = button;
        HidePriceUI(button);
        button.GetComponent<RectTransform>().localPosition += transform.up * (DIF-10)/2;
        ForSaleVeh.GetComponent<RectTransform>().localPosition += transform.up * (-DIF / 2);
        vehicles[index] = 1;
        if (index == 0 && vehicles[1] == 0) vehbut2.onClick.AddListener(() => TaskOnClickBuyVeh(1, vehbut2));
        button.onClick.RemoveAllListeners();
    }
    void TaskOnClickBuyVeh(int index, Button button)//vehicles
    {
        int price = Convert.ToInt32(button.transform.GetChild(1).gameObject.GetComponent<Text>().text);
        if (price <= Coins())
        {
            BoughtVehicle(index, button);

            MinusCoins(price);
            ReCoins();
            Saver();
        }
    }
    void HidePriceUI(Button button)
    {
        button.transform.GetChild(1).gameObject.SetActive(false);
        button.transform.GetChild(2).gameObject.SetActive(false);
    }

    //MAIN MENU BUTTONS
    void DefaultUIHide()
    {
        BG.gameObject.SetActive(true);
        HideObj1.gameObject.SetActive(false);
        HideObj2.gameObject.SetActive(false);
    }
    void TaskOnCostumes()
    {
        DefaultUIHide();
        CostumesMenu.gameObject.SetActive(true);
        JetpacksMenu.gameObject.SetActive(false);
        VehiclesMenu.gameObject.SetActive(false);
        GadgetsMenu.gameObject.SetActive(false);
    }
    void TaskOnJetPacks()
    {
        DefaultUIHide();
        JetpacksMenu.gameObject.SetActive(true);
        CostumesMenu.gameObject.SetActive(false);
        VehiclesMenu.gameObject.SetActive(false);
        GadgetsMenu.gameObject.SetActive(false);
    }
    void TaskOnVehicles()
    {
        DefaultUIHide();
        VehiclesMenu.gameObject.SetActive(true);
        CostumesMenu.gameObject.SetActive(false);
        JetpacksMenu.gameObject.SetActive(false);
        GadgetsMenu.gameObject.SetActive(false);
    }
    void TaskOnGadgets()
    {
        DefaultUIHide();
        GadgetsMenu.gameObject.SetActive(true);
        CostumesMenu.gameObject.SetActive(false);
        JetpacksMenu.gameObject.SetActive(false);
        VehiclesMenu.gameObject.SetActive(false);
    }

    //ADDITIONAL func
    int Coins()
    {
        coins = Convert.ToInt32(File.ReadAllText(Application.dataPath + "/Coins.txt"));
        return coins;
    }

    void MinusCoins(int price)
    {
        coins -= price;
        File.WriteAllText(Application.dataPath + "/Coins.txt", $"{coins}");
        canv.ReNewCoin(coins);
    }

    void ReCoins()
    {
        CoinsField.gameObject.GetComponent<Text>().text = Coins().ToString();
    }

    public void Spliter()
    {
        string name;
        if (vehicles.Length == 1) vehicles = new int[2] { 0, 0 };
        dict.Add("button", button);
        dict.Add("button1", button1);

        dict.Add("jetbut1", JetBut[0]);
        dict.Add("jetbut2", JetBut[1]);
        dict.Add("jetbut3", JetBut[2]);
        dict.Add("jetbut4", JetBut[3]);
        dict.Add("jetbut5", JetBut[4]);

        dict.Add("vehbut1", vehbut1);
        dict.Add("vehbut2", vehbut2);

        for (int i = 0; i < items[0].Split('-').Length; i++)
        {
            name = items[0].Split('-')[i];
            if(dict.ContainsKey(name))
            {
                boughtItem[i] = dict[name];
            }
        }
        for (int i = 0; i < items[1].Split('-').Length; i++)
        {
            name = items[1].Split('-')[i];
            if (dict.ContainsKey(name))
            {
                boughtItemJet[i] = dict[name];
            }
        }
        for (int i = 0; i < items[2].Split('-').Length; i++)
        {
            name = items[2].Split('-')[i];
            if (dict.ContainsKey(name))
            {
                boughtItemVeh[i] = dict[name];
            }
        }    

        foreach (Button but in boughtItem)
        {
            if(but == button)
            {
                Button btn1 = button1.GetComponent<Button>();
                btn1.onClick.AddListener(TaskOnClickBuy);//buy second costume
                Button but1 = but.GetComponent<Button>();
                but1.onClick.AddListener(() => ChooseSkin(Costume1));
            }  
            else if (but == button1)
            {
                BoughtCostume();
            }
        }

        foreach (Button but in boughtItemJet)
        {
            if (but == JetBut[0])
            {
                JetBut[1].onClick.AddListener(() => TaskOnClickBuyJet(1, JetBut[1]));//buy second jet
                but.onClick.AddListener(() => ChooseJet(0));
            }
            else if (but == JetBut[1])
            {
                BoughtJet(1, but);
            }
            else if (but == JetBut[2])
            {
                BoughtJet(2, but);
            }
            else if (but == JetBut[3])
            {
                BoughtJet(3, but);
            }
            else if (but == JetBut[4])
            {
                BoughtJet(4, but);
            }
        }
        bool HaveVeh = false;
        foreach (Button but in boughtItemVeh)
        {
            if (but == vehbut1)
            {
                BoughtVehicle(0, vehbut1);
                HaveVeh = true;
            }
            else if (but == vehbut2)
            {
                BoughtVehicle(1, vehbut2);
                HaveVeh = true;
            }
            else if (!HaveVeh)
            {
                Button vbut1 = vehbut1.GetComponent<Button>();
                vbut1.onClick.AddListener(() => TaskOnClickBuyVeh(0, vehbut1));
            }
        }
        playerController.Veh();
        RandomVeh();
    }
    void Saver()
    {
        string name = "abc";
        string line = "";
        string line2 = "";
        string line3 = "";

        for (int i = 0; i < boughtItem.Length; i++)
        {

            name = boughtItem[i].name.ToLower();
            if (dict.ContainsKey(name))
            {
                line += name + "-";
            }
        }
        for (int i = 0; i < boughtItemJet.Length; i++)
        {
            if (boughtItemJet[i] == null) break;
            name = boughtItemJet[i].name.ToLower();
            if (dict.ContainsKey(name))
            {
                line2 += name + "-";
            }
        }
        for (int i = 0; i < boughtItemVeh.Length; i++)
        {
            if (boughtItemVeh[i] == null) break;
            name = boughtItemVeh[i].name.ToLower();
            if (dict.ContainsKey(name))
            {
                line3 += name + "-";
            }
        }

        IEnumerable<string> str = new string[] { line, line2, line3 };

        File.WriteAllLines(path, str);

    }

    public void RandomVeh()
    {
        int checker = -1;
        for (int i = 0; i < vehicles.Length; i++)
        {
            if (vehicles[i] == 1)
            {
                checker++;
            }
        }
        if (checker >= 0)
        { 
            playerController.vehicleActive = UnityEngine.Random.Range(0, checker + 1);
            playerController.vehicleChecker = checker + 1;
        }
    }
}
// pomer vid krinzi ;)