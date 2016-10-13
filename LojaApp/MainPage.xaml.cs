using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LojaApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //public string ip = "http://10.21.0.137";
        public string ip = "http://localhost:50626/";
        List<Models.Fabricante> fabValera;
        public MainPage()
        {
            this.InitializeComponent();
            populateCmbox();
            getFabricantes();
            getVeiculos();

            fabValera = new List<Models.Fabricante>();
        }

        private void btnInserir_Click(object sender, RoutedEventArgs e)
        {
            Models.Loja db = new Models.Loja();
            Models.Fabricante f = new Models.Fabricante
            {
                Id = int.Parse(txtIdFab.Text),
                Descricao = txtDesc.Text
            };
            db.Fabricantes.Add(f);
            db.SaveChanges();
            lstFabricantes.ItemsSource = db.Fabricantes.ToList();
        }

        private void btnListFab_Click(object sender, RoutedEventArgs e)
        {
            getFabricantes();
        }

        public List<Models.Fabricante> getFabricantes()
        {
            Models.Loja db = new Models.Loja();
            lstFabricantes.ItemsSource = db.Fabricantes.ToList();
            return db.Fabricantes.ToList();
        }


        public async void getFabValera()
        {

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("api/fabricante");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Fabricante> obj = JsonConvert.DeserializeObject<List<Models.Fabricante>>(str);
            lstFabricantes.ItemsSource = obj;
            Models.Loja db = new Models.Loja();
            foreach (Models.Fabricante f in obj)
            { 
                db.Fabricantes.Add(f);
                db.SaveChanges();
            }
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            getFabValera();
        }

        List<Models.Veiculo> veiValera;
        public async void getVeicValera()
        {
            veiValera = new List<Models.Veiculo>();
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("api/veiculo");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Veiculo> obj = JsonConvert.DeserializeObject<List<Models.Veiculo>>(str);
            veiValera = obj.ToList();
        }

        public List<Models.Veiculo> getVeiculos()
        {
            Models.Loja db = new Models.Loja();
            lstVeic.ItemsSource = db.Veiculos.ToList();
            lstVeicDisp.ItemsSource = db.Veiculos.ToList();
            var vendidos = from Models.Veiculo v in db.Veiculos where v.Vendido == true select v;
            lstveicVendidos.ItemsSource = vendidos.ToList();
            return db.Veiculos.ToList();
        }

        public void populateCmbox()
        {
            cmbFabricantes.ItemsSource = getFabricantes();
            cmbFabricantes.SelectedValuePath = "Id";
            cmbFabricantes.DisplayMemberPath = "Descricao";
        }


        private void btnInserirVeic_Click(object sender, RoutedEventArgs e)
        {
            bool k;
            if ((bool)checkBox.IsChecked)
                k = true;
            else
                k = false;
            Models.Loja db = new Models.Loja();
            Models.Veiculo v = new Models.Veiculo
            {
                Id = int.Parse(txtIdVeic.Text),
                Modelo = txtModelo.Text,
                Ano = int.Parse(txtAno.Text),
                Preco = double.Parse(txtPreco.Text),
                IdFabricante = int.Parse(cmbFabricantes.SelectedValue.ToString()),
                Vendido = k
            };

            db.Veiculos.Add(v);
            db.SaveChanges();
            getVeiculos();
        }

        private void btnVender_Click(object sender, RoutedEventArgs e)
        {
            Models.Loja db = new Models.Loja();
            var ListaVeiculos = getVeiculos();
            Models.Veiculo v = (from Models.Veiculo f in ListaVeiculos where f.Id == int.Parse(txtIdVeic.Text) select f).Single();
            v.Vendido = true;
            db.Veiculos.Update(v);
            db.SaveChanges();
        }

        
    }
}
