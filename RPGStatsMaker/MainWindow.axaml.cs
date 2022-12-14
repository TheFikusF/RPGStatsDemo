using System;
using System.Reactive.Subjects;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using RPGStatsMaker.Stats;

namespace RPGStatsMaker
{
    public partial class MainWindow : Window
    {
        private Character _player;
        private Subject<string> _firedamage;
        public MainWindow()
        {
            _firedamage = new Subject<string>();
            _player = new PlayerCharacter();
            var num = _player.Stats.GetDamageValue(100, SourceType.Spell, DamageType.Fire);
            InitializeComponent();
            StatTypeBox.Items = new[]{ typeof(DamageStat), typeof(CooldownReductionStat), typeof(CastSpeedStat)};
            SelfTypeBox.Items = Enum.GetValues<SelfType>();
            DamageTypeBox.Items = Enum.GetValues<DamageType>();
            SourceTypeBox.Items = Enum.GetValues<SourceType>();
            IncreaseTypeBox.Items = Enum.GetValues<IncreasType>();
            StatTypeBox.SelectedIndex = 0;
            SelfTypeBox.SelectedIndex = 0;
            DamageTypeBox.SelectedIndex = 0;
            SourceTypeBox.SelectedIndex = 0;
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
                    DamageTypeBox.IsEnabled = true;
                    IncreaseTypeBox.IsEnabled = true;
                    break;
                case 1:
                    DamageTypeBox.IsEnabled = false;
                    IncreaseTypeBox.IsEnabled = false;
                    break;
                case 2:
                    DamageTypeBox.IsEnabled = false;
                    IncreaseTypeBox.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        private Stat GetNewStat()
        {
            double statValue = 0;
            if(!double.TryParse(ValueBox.Text, out statValue)) return null;
            return StatTypeBox.SelectedIndex switch
            {
                0 => new DamageStat(statValue,
                    (SelfType) SelfTypeBox.SelectedItem,
                    (SourceType) SourceTypeBox.SelectedItem,
                    (DamageType) DamageTypeBox.SelectedItem, 
                    (IncreasType) IncreaseTypeBox.SelectedItem),
                1 => new CooldownReductionStat(statValue,
                    (SelfType) SelfTypeBox.SelectedItem,
                    (SourceType) SourceTypeBox.SelectedItem),
                2 => new CastSpeedStat(statValue,
                    (SelfType) SelfTypeBox.SelectedItem,
                    (SourceType) SourceTypeBox.SelectedItem),
                _ => null
            };
        }

        private bool TryGetNewStat(out Stat stat)
        {
            stat = GetNewStat();
            return stat != null;
        }

        private void CreateStatLabel(Stat s)
        {
            var statLabel = new Label();
            var removeButton = new Button();
            var panel = new StackPanel();

            void RemoveButtonFunc(object? sender, RoutedEventArgs e)
            {
                _player.Stats.RemoveStat(s);
                var num = _player.Stats.GetDamageValue(100, SourceType.Spell, DamageType.Fire);
                _firedamage.OnNext("damage: " + Math.Round(num, 2));
                MainStack.Children.Remove(panel);
                removeButton.Click -= RemoveButtonFunc;
                removeButton = null;
                statLabel = null;
                panel = null;
            }
            
            removeButton.Content = "-";
            removeButton.Click += RemoveButtonFunc;

            statLabel.Content = s.ToString();
            statLabel.Width = 350;
            MainStack.Children.Add(panel);
            panel.Orientation = Orientation.Horizontal;
            panel.Children.Add(statLabel);
            panel.Children.Add(removeButton);
            panel.Margin = Thickness.Parse("5");
        }
        
        private void AddAction(object? sender, RoutedEventArgs e)
        {
            if(!TryGetNewStat(out Stat s)) return;
            
            _player.Stats.AddStat(s);
            CreateStatLabel(s);
            var num = _player.Stats.GetDamageValue(100, SourceType.Spell, DamageType.Fire);
            _firedamage.OnNext("damage: " + Math.Round(num, 2));
        }
    }
}