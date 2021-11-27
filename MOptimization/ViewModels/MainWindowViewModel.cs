using MOptimization.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace MOptimization.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public DelegateCommand CalculateCommand;
        public MSOptimizationModel model;

        private string _output = "Output";
        private double[] _startPoint = { 0, 0, 0 };
        private double _step = 1;
        private double _eps = 0.001;

        /*public string Output
        {
            get => _output;
            set => SetProperty(ref _output, value);
        }

        public string StartPoint
        {
            get => _startPoint.ToString();
            set => {
                SetProperty(ref _startPoint, value);
            };
        }

        public string Step
        {
            get => _step;
            set => SetProperty(ref _step, value);
        }

        public string Eps
        {
            get => _step;
            set => SetProperty(ref _step, value);
        }*/

        public MainWindowViewModel()
        {
            CalculateCommand = new DelegateCommand(Calculate);
            model = new MSOptimizationModel();
        }

        public void Calculate()
        {

        }
    }
}
