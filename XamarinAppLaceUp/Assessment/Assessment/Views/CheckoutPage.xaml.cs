using System.Threading.Tasks;
using System;
using Xamarin.Forms;

namespace Assessment.Views
{
    public partial class CheckoutPage : ContentPage
    {
        public CheckoutPage()
        {
            InitializeComponent();
        }
        private async void ViewCell_Appearing(object sender, EventArgs e)
        {
            var cell = sender as ViewCell;
            var view = cell.View;

            view.TranslationX = -100;
            view.Opacity = 0;

            await Task.WhenAny<bool>
            (
                view.TranslateTo(0, 0, 500, Easing.SinIn),
                view.FadeTo(1, 500, Easing.SinIn)
            );
        }
    }
}
