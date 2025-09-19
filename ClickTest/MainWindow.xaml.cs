using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace ReactionTimeTest
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer countdownTimer;
        private DateTime signalStartTime;
        private List<double> reactionTimes;
        private int currentTest;
        private const int totalTests = 5;
        private Random random;
        private int countdownValue;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            countdownTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            countdownTimer.Tick += CountdownTimer_Tick;

            random = new Random();
            reactionTimes = new List<double>();
            currentTest = 0;

            UpdateResultsDisplay();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewTest();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }

        private void ReactionButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReactionButton.IsEnabled)
            {
                RecordReactionTime();
            }
        }

        private void StartNewTest()
        {
            if (currentTest >= totalTests)
            {
                ShowFinalResults();
                return;
            }

            StartButton.IsEnabled = false;
            ReactionButton.IsEnabled = false;
            
            SignalArea.Background = new SolidColorBrush(Color.FromRgb(220, 220, 220));
            StatusText.Text = $"Тест {currentTest + 1}/{totalTests}\nПриготовьтесь...";
            StatusText.Foreground = new SolidColorBrush(Color.FromRgb(105, 105, 105));

            // Случайная задержка от 2 до 5 секунд
            int delaySeconds = random.Next(2, 6);
            countdownValue = delaySeconds;
            
            StatusText.Text = $"Тест {currentTest + 1}/{totalTests}\nСигнал через: {countdownValue} сек";
            
            countdownTimer.Start();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            countdownValue--;
            
            if (countdownValue > 0)
            {
                StatusText.Text = $"Тест {currentTest + 1}/{totalTests}\nСигнал через: {countdownValue} сек";
            }
            else
            {
                countdownTimer.Stop();
                ShowSignal();
            }
        }

        private void ShowSignal()
        {
            SignalArea.Background = new SolidColorBrush(Color.FromRgb(50, 205, 50));
            StatusText.Text = "НАЖИМАЙТЕ СЕЙЧАС!";
            StatusText.Foreground = Brushes.White;
            StatusText.FontSize = 24;
            ReactionButton.IsEnabled = true;
            signalStartTime = DateTime.Now;
        }

        private void RecordReactionTime()
        {
            if (signalStartTime != DateTime.MinValue)
            {
                double reactionTime = (DateTime.Now - signalStartTime).TotalMilliseconds;
                reactionTimes.Add(reactionTime);
                
                SignalArea.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
                StatusText.Text = $"Время реакции:\n{reactionTime:F0} мс";
                StatusText.Foreground = Brushes.White;
                StatusText.FontSize = 20;
                ReactionButton.IsEnabled = false;
                
                currentTest++;
                
                UpdateResultsDisplay();
                
                // Пауза перед следующим тестом
                DispatcherTimer pauseTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(2.5)
                };
                pauseTimer.Tick += (s, e) =>
                {
                    pauseTimer.Stop();
                    StatusText.FontSize = 18; // Возвращаем исходный размер
                    
                    if (currentTest < totalTests)
                    {
                        StartButton.IsEnabled = true;
                        SignalArea.Background = new SolidColorBrush(Color.FromRgb(220, 220, 220));
                        StatusText.Text = "Нажмите 'Начать тест' для следующего теста";
                        StatusText.Foreground = new SolidColorBrush(Color.FromRgb(105, 105, 105));
                    }
                    else
                    {
                        ShowFinalResults();
                    }
                };
                pauseTimer.Start();
            }
        }

        private void UpdateResultsDisplay()
        {
            if (reactionTimes.Count > 0)
            {
                double average = reactionTimes.Average();
                double min = reactionTimes.Min();
                double max = reactionTimes.Max();
                
                ResultsText.Text = $"Тестов: {reactionTimes.Count}/{totalTests}\n" +
                                 $"Среднее время: {average:F0} мс\n" +
                                 $"Лучший результат: {min:F0} мс | Худший: {max:F0} мс";
            }
            else
            {
                ResultsText.Text = $"Тестов: 0/{totalTests}\nГотов к началу тестирования";
            }
        }

        private void ShowFinalResults()
        {
            if (reactionTimes.Count > 0)
            {
                double average = reactionTimes.Average();
                double min = reactionTimes.Min();
                double max = reactionTimes.Max();
                
                string performance = GetPerformanceRating(average);
                string performanceColor = GetPerformanceColor(average);
                
                SignalArea.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(performanceColor));
                StatusText.Text = $"🏁 ТЕСТ ЗАВЕРШЕН! 🏁\n\n{performance}\n\n" +
                                $"📊 Среднее время: {average:F0} мс\n" +
                                $"🏆 Лучший результат: {min:F0} мс\n" +
                                $"📈 Диапазон: {min:F0}-{max:F0} мс";
                StatusText.Foreground = Brushes.White;
                StatusText.FontSize = 16;
            }
            
            StartButton.IsEnabled = true;
            StartButton.Content = "Новый тест";
        }

        private string GetPerformanceRating(double average)
        {
            if (average < 200) return "⚡ НЕВЕРОЯТНО!";
            else if (average < 250) return "🎯 ОТЛИЧНО!";
            else if (average < 300) return "👍 ОЧЕНЬ ХОРОШО!";
            else if (average < 350) return "✅ ХОРОШО!";
            else if (average < 400) return "👌 НОРМАЛЬНО";
            else if (average < 500) return "📚 ЕСТЬ К ЧЕМУ СТРЕМИТЬСЯ";
            else return "🐌 НУЖНО ТРЕНИРОВАТЬСЯ";
        }

        private string GetPerformanceColor(double average)
        {
            if (average < 200) return "#FF1493"; 
            else if (average < 250) return "#FF6347"; 
            else if (average < 300) return "#32CD32"; 
            else if (average < 350) return "#4169E1"; 
            else if (average < 400) return "#9370DB"; 
            else if (average < 500) return "#FF8C00"; 
            else return "#B22222"; 
        }

        private void ResetGame()
        {
            countdownTimer.Stop();
            
            reactionTimes.Clear();
            currentTest = 0;
            signalStartTime = DateTime.MinValue;
            
            SignalArea.Background = new SolidColorBrush(Color.FromRgb(220, 220, 220));
            StatusText.Text = "Нажмите 'Начать тест' для старта";
            StatusText.Foreground = new SolidColorBrush(Color.FromRgb(105, 105, 105));
            StatusText.FontSize = 18;
            
            ReactionButton.IsEnabled = false;
            StartButton.IsEnabled = true;
            StartButton.Content = "Начать тест";
            
            UpdateResultsDisplay();
        }
    }
}