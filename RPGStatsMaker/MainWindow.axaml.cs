using System;
using System.Reactive.Subjects;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace RPGStatsMaker
{
    public partial class MainWindow : Window
    {
        private PlayerStats _playerStats;
        private Subject<string> _firedamage;
        public MainWindow()
        {
            _firedamage = new Subject<string>();
            _playerStats = new PlayerStats();
            var num = _playerStats.GetDamageValue(100, SourceType.Spell, DamageType.Fire);
            InitializeComponent();
            StatTypeBox.Items = new[]{ typeof(DamageStat), typeof(CooldownReductionStat), typeof(CastSpeedStat)};
            SelfTypeBox.Items = Enum.GetValues<SelfType>();
            DamageTypeBox.Items = Enum.GetValues<DamageType>();
            IncreaseTypeBox.Items = Enum.GetValues<IncreasType>();
            StatTypeBox.SelectedIndex = 0;
            SelfTypeBox.SelectedIndex = 0;
            DamageTypeBox.SelectedIndex = 0;
            IncreaseTypeBox.SelectedIndex = 0;
            mainLabel.Bind(Label.ContentProperty, _firedamage);
            _firedamage.OnNext("damage: " + Math.Round(num, 2));
        }

        private void StatTypeChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if(IsInitialized == false) return;
            
            switch (StatTypeBox.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }

        private void AddAction(object? sender, RoutedEventArgs e)
        {
            var l = new Label();
            var b = new Button();
            var panel = new StackPanel();
            Stat s = null;
            double statValue = 0;
            if(!double.TryParse(ValueBox.Text, out statValue)) return;
            switch (StatTypeBox.SelectedIndex)
            {
                case 0:
                    var stat = new DamageStat(statValue,
                        (SelfType) SelfTypeBox.SelectedItem,
                        SourceType.Any,
                        (DamageType) DamageTypeBox.SelectedItem, 
                        (IncreasType) IncreaseTypeBox.SelectedItem);
                    _playerStats.AddStat(stat);
                    s = stat;
                    break;
                case 1:
                    return;
                    break;
                case 2:
                    return;
                    break;
                default:
                    return;
                    break;
            }

            b.Content = "-";
            b.Click += (object? sender, RoutedEventArgs e) =>
            {
                _playerStats.RemoveStat(s);
                var num = _playerStats.GetDamageValue(100, SourceType.Spell, DamageType.Fire);
                _firedamage.OnNext("damage: " + Math.Round(num, 2));
                MainStack.Children.Remove(panel);
                b = null;
                l = null;
                panel = null;
            };
            
            l.Content = s.ToString();
            l.Width = 300;
            var num = _playerStats.GetDamageValue(100, SourceType.Spell, DamageType.Fire);
            _firedamage.OnNext("damage: " + Math.Round(num, 2));
            MainStack.Children.Add(panel);
            panel.Children.Add(l);
            panel.Orientation = Orientation.Horizontal;
            panel.Children.Add(b);
            panel.Margin = Thickness.Parse("5");
        }
    }
}