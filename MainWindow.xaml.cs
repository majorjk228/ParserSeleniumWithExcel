using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParserSelenium
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, string> _dict; //создаем словарь куда записываем данные
        public MainWindow()
        {
            InitializeComponent();

            _dict = new Dictionary<string, string>();
        }

        private async void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            /*  if (DateFrom.SelectedDate == null || DateTo.SelectedDate == null || string.IsNullOrEmpty(ArticleTb.Text))
              {
                  MessageBox.Show("Не выбрана дата начала или окончания или номер статьи!");
                  return;
              }*/

            //var dateFrom = (DateTime)DateFrom.SelectedDate;
            // var dateTo = (DateTime)DateTo.SelectedDate;
            var article = ArticleTb.Text;

            IWebDriver driver = new ChromeDriver(); //создаем хром бразуер
            driver.Url = @"https://realty.yandex.ru/moskva_i_moskovskaya_oblast/agentstva/?rgid=587795";


            for (int i = 0; i < 10; i++)
            {
                try
                {
                    driver.FindElement(By.XPath("//span[contains(.,'Сбросить ⋅ 1')]")).Click();
                    await Task.Delay(1000); //ждем секунду
                    break;
                }
                catch (Exception)
                {
                    await Task.Delay(1000); //ждем секунду если не может найти
                }
            }

            for (int i = 0; i < 10; i++) //Поиск по городу
            {
                try
                {
                    driver.FindElement(By.XPath("//span[@class='RegionSelect__value RegionSelect__value-withIcon']")).Click();
                    await Task.Delay(1000); //ждем секунду
                    driver.FindElement(By.XPath("(//input[@type='text'])[4]")).SendKeys(article); //Задаем город
                    await Task.Delay(1000); //ждем секунду
                    driver.FindElement(By.XPath("(//input[@type='text'])[4]")).SendKeys(Keys.Enter);
                    await Task.Delay(500); //ждем секунду
                    driver.FindElement(By.XPath("//span[@class='Button__text'][contains(.,'Сохранить')]")).Click();
                    break;
                }
                catch (Exception)
                {
                    await Task.Delay(1000); //ждем секунду если не может найти
                }
            }
            await Task.Delay(1500);

            /*   await Task.Delay(1500);
               var result1 = driver.FindElement(By.XPath("(//p[contains(@itemprop,'name')])[2]")).GetAttribute("textContent");
               await Task.Delay(1500);
               driver.FindElement(By.XPath("(//div[contains(@class,'textCentered--2weaN')])[2]")).Click();
               await Task.Delay(500);
               var result2 = driver.FindElement(By.XPath($"(//div[contains(@class,'buttonContent--2h4Ye')])[{2}]")).GetAttribute("textContent");
               await Task.Delay(500);
               MessageBox.Show(result1);
               MessageBox.Show(result2);*/
            for (int i = 1; i < 10; i++) //Поиск по городу
            {
                try
                {
                    await Task.Delay(1000);
                    var nameCompany = driver.FindElement(By.XPath($"(//p[contains(@itemprop,'name')])[{i}]")).GetAttribute("textContent");
                    await Task.Delay(500);
                    driver.FindElement(By.XPath($"(//div[contains(@class,'textCentered--2weaN')])[{i}]")).Click();
                    await Task.Delay(500);
                    var numberTel = driver.FindElement(By.XPath($"//span[@class='PhoneButtonWithQR__buttonTextContent--1KEJE']")).GetAttribute("textContent");
                    await Task.Delay(1500);

                    // MessageBox.Show(result1);

                    _dict[nameCompany] = numberTel; //ключ номер компании, значение словаря номер телефона

                    driver.Navigate().Refresh();


                   // driver.FindElement(By.XPath("")).Click(); нажать кнопку вперед чтобы перейти на след страницу
                }
                catch (Exception)
                {
                    await Task.Delay(1000); //ждем секунду если не может найти
                }
            }

           // driver.FindElement(By.XPath("//label[contains(.,'Дело')]")).Click();  переход на следующую страницу
            /* var oldNumber = string.Empty;

             var counter = 0;

             while (true)
             {
                 await Task.Delay(2500);

                 counter++;

                 if (counter == 6) //Антикапча
                 {
                     break;
                 }        

                 try
                 {
                     driver.FindElement(By.XPath("//label[contains(.,'Дело')]")).Click();
                 }
                 catch (Exception)
                 {                
                 }

                 await Task.Delay(2500);

                 var number = driver.FindElement(By.XPath("(//span[@data-pos='0'])[1]")).GetAttribute("textContent");//забираем текст который лежит в контейнере

                 var region = driver.FindElement(By.XPath("(//span[@data-pos='0'])[6]")).GetAttribute("textContent");//забираем текст который лежит в контейнере

                 if (number == oldNumber) // если значение намбера пустое, то остановим цикл(проходим до конца, чтоб цикл не уходил в бесконечность
                 {
                     break;
                 }

                 _dict[number] = region; //ключ номер дела, значение словаря регион 

                 await Task.Delay(2500);

                 driver.FindElement(By.XPath("(//span[contains(@title,'Вперед')])[3]")).Click();

                 await Task.Delay(2500);
             }*/

            driver.Quit();

            var result = string.Empty;
            foreach (var pair in _dict)
            {
                result += $"{pair.Key}={pair.Value}\r\n"; //в переменную запишутся все строки
            }

             MessageBox.Show(result);
        }
    }
}
