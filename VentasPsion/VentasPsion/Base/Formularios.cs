using Android.Widget;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Plugin.CurrentActivity;
using Android.App;
using Android.Views;
using System.Collections;

namespace VentasPsion.Base
{
    public class Formularios
    {
        Activity oActivity = CrossCurrentActivity.Current.Activity;
        public Formularios()
        {

        }

        public Xamarin.Forms.Button CrearButton(bool _IsEnabled, bool _IsVisible, string _Text, LayoutOptions _LayoutOptions, string _Image)
        {
            return new Xamarin.Forms.Button
            {
                IsEnabled = _IsEnabled,
                IsVisible = _IsVisible,
                Text = _Text,
                HorizontalOptions = _LayoutOptions,
                Image = _Image 
            };
        }

        public Entry CrearEntry(bool _IsEnabled, bool _IsVisible, string _Text, string _Placeholder)
        {
            return new Entry
            {
                IsEnabled = _IsEnabled,
                IsVisible = _IsVisible,
                Text = _Text,
                Placeholder = _Placeholder
                
            };
        }

        public Editor CrearEditor(bool _IsEnabled, bool _IsVisible, string _Text, int HeightRequest,string _ClassId)
        {
            return new Editor
            {
                IsEnabled = _IsEnabled,
                IsVisible = _IsVisible,
                Text = _Text,
                HeightRequest = HeightRequest,
                ClassId = _ClassId
            };
        }

        public Label CrearLabel(bool _IsEnabled, bool _IsVisible, string _Text)
        {
            return new Label
            {
                IsEnabled = _IsEnabled,
                IsVisible = _IsVisible,
                Text = _Text
            };
        }

        public Android.Widget.Switch CrearSwitch(bool _IsEnabled,string _TextoTrue, string _TextoFalse, string _Tag)
        {
            Android.Widget.Switch oSwitch = new Android.Widget.Switch(oActivity);
            oSwitch.TextOn = _TextoTrue;
            oSwitch.TextOff = _TextoFalse;
            oSwitch.Enabled = _IsEnabled;
            oSwitch.Gravity = GravityFlags.Center;
            oSwitch.LayoutParameters = new ViewGroup.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
            oSwitch.ShowText = true;
            oSwitch.Tag = _Tag;
            return oSwitch;
        }


        public Picker CrearPicker(bool _IsEnabled, bool _IsVisible,string _Title, Dictionary<string, bool> _Dictionary)
        {
            Picker oPicker = new Picker();
            oPicker.IsEnabled = _IsEnabled;
            oPicker.IsVisible = _IsVisible;
            oPicker.Title = _Title;
            oPicker.HorizontalOptions = LayoutOptions.FillAndExpand;
            oPicker.VerticalOptions = LayoutOptions.FillAndExpand;
            foreach (string oDiccionario in _Dictionary.Keys)
            {
                oPicker.Items.Add(oDiccionario);
            }

            return oPicker;
        }
        public Android.Widget.CheckBox CrearCheckBox(string _Text, bool _Enabled, ViewStates _Visible, string _Tag,string _ContentDescription)
        {
            Android.Widget.CheckBox oCheckBox = new Android.Widget.CheckBox(oActivity);
            oCheckBox.Text = _Text;
            oCheckBox.Enabled = _Enabled;
            oCheckBox.Visibility = _Visible;
            oCheckBox.Tag = _Tag;
            oCheckBox.ContentDescription = _Tag;
            oCheckBox.LayoutParameters = new ViewGroup.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
            return oCheckBox;

        }
        public Android.Widget.RadioButton CrearRadioButton(string _Text, bool _Enabled, ViewStates _Visible, string _Tag, string _ContentDescription)
        {
            Android.Widget.RadioButton oRadioButton = new Android.Widget.RadioButton(oActivity);
            oRadioButton.Text = _Text;
            oRadioButton.Enabled = _Enabled;
            oRadioButton.Visibility = _Visible;
            oRadioButton.Tag = _Tag;
            oRadioButton.ContentDescription = _Tag;
            oRadioButton.LayoutParameters = new ViewGroup.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
            return oRadioButton;
        }

        public Android.Widget.RadioGroup CrearRadioGroup(bool _Enabled, ViewStates _Visible, string _Tag)
        {
            Android.Widget.RadioGroup oRadioButton = new Android.Widget.RadioGroup(oActivity);
            oRadioButton.Enabled = _Enabled;
            oRadioButton.Visibility = _Visible;
            oRadioButton.Tag = _Tag;
            oRadioButton.LayoutParameters = new ViewGroup.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
            return oRadioButton;
        }

        public Android.Widget.TimePicker CrearTimePicker(bool _Enabled, ViewStates _Visible, string _Tag)
        {
            Android.Widget.TimePicker oTimerPicker = new Android.Widget.TimePicker(oActivity);
            oTimerPicker.Enabled = _Enabled;
            oTimerPicker.Visibility = _Visible;
            oTimerPicker.Tag = _Tag;
            oTimerPicker.LayoutParameters = new ViewGroup.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
            return oTimerPicker;
        }
    }
}