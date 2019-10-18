using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StraticatorFroms_iOS
{
    public class CustomStepperRenderer : Stepper
    {

    }

    //    Solution 1:
    //A Stepper allows inputting a discrete value that is constrained to a range.You could display the value of the Stepper using data binding in a label as follows :

    //Define in XAML:

    //<StackLayout x:Name="Container">
    //    <Label BindingContext = "{x:Reference stepper}" Text="{Binding Value}" />
    //    <Stepper Minimum = "0" Maximum="10" x:Name="stepper" Increment="0.5" />
    //</StackLayout>
    //Solution 2:
    //You could create a BindableProperty to implement this function, for example:

    public class CustomStepperTest : StackLayout
    {
        Button PlusBtn;
        Button MinusBtn;
        CustomEntry Entry;

        public static readonly BindableProperty TextProperty =
          BindableProperty.Create(
             propertyName: "Text",
              returnType: typeof(int),
              declaringType: typeof(CustomStepperTest),
              defaultValue: 1,
              defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty IncrementProperty =
          BindableProperty.Create(
             propertyName: "Increment",
              returnType: typeof(int),
              declaringType: typeof(CustomStepperTest),
              defaultValue: 1,
              defaultBindingMode: BindingMode.TwoWay);

        public int Text
        {
            get { return (int)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public int Increment
        {
            get { return (int)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }
        public CustomStepperTest()
        {
            PlusBtn = new Button { Text = "+", WidthRequest = 60, FontAttributes = FontAttributes.Bold,HeightRequest= 40, FontSize = 15 };
            MinusBtn = new Button { Text = "-", WidthRequest = 60, FontAttributes = FontAttributes.Bold, HeightRequest = 40, FontSize = 15 };
            switch (Device.RuntimePlatform)
            {

                case Device.UWP:
                case Device.Android:
                    {
                        PlusBtn.BackgroundColor = Color.Transparent;
                        MinusBtn.BackgroundColor = Color.Transparent;
                        break;
                    }
                case Device.iOS:
                    {
                        PlusBtn.BackgroundColor = Color.DarkGray;
                        MinusBtn.BackgroundColor = Color.DarkGray;
                        break;
                    }
            }

            Orientation = StackOrientation.Horizontal;
            PlusBtn.Clicked += PlusBtn_Clicked;
            MinusBtn.Clicked += MinusBtn_Clicked;
            Entry = new CustomEntry
            {
                PlaceholderColor = Color.Gray,
                Keyboard = Keyboard.Numeric,
                WidthRequest = 220,
                HeightRequest = 40,
                BackgroundColor = Color.FromHex("#3FFF")
            };
            Entry.SetBinding(CustomEntry.TextProperty, new Binding(nameof(Text), BindingMode.TwoWay, source: this));
            Entry.TextChanged += Entry_TextChanged;
            Children.Add(Entry);
            Children.Add(PlusBtn);
            Children.Add(MinusBtn);
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
                this.Text = int.Parse(e.NewTextValue);
        }

        private void MinusBtn_Clicked(object sender, EventArgs e)
        {
            if (Increment == 5)
                Text -= 5;
            else
                Text--;
        }

        private void PlusBtn_Clicked(object sender, EventArgs e)
        {
            if (Increment == 5)
                Text += 5;
            else
                Text++;
        }

    }
}
