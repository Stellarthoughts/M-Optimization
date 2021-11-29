namespace MSOptimization.ViewModels
{
    using MSOptimization.Core;
    using MSOptimization.Models;
    using MSOptimization.NumericMethods;
    using Prism.Commands;
	using Prism.Mvvm;
	using System;
	using System.Globalization;

	public class MainWindowViewModel : BindableBase
	{
		// Window settings
		private string _title;

		public string Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}

		// Input settings
		private CultureInfo _cultureInfo = CultureInfo.InvariantCulture;

		// Optimization
		private string _output;
		private double[] _startPoint;
		private int _maxIter;
		private double _eps;
		private MSOptimizationModel _model;

		public string Output
		{
			get => _output;
			set => SetProperty(ref _output, value);
		}

		public string StartPoint
		{
			get => string.Join(" ", _startPoint);
			set {
				try {
					SetProperty(ref _startPoint, Array.ConvertAll(value.Split(" "), Double.Parse));
				}
				catch { return; }
			}
		}

		public string MaxIterations
		{
			get => _maxIter.ToString();
			set {
				try {
					SetProperty(ref _maxIter, Int32.Parse(value));
				}
				catch { return; }
			}
		}

		public string Eps
		{
			get => _eps.ToString(_cultureInfo);
			set {
				try {
					SetProperty(ref _eps, Double.Parse(value, _cultureInfo));
				}
				catch { return;  }
			}
		}

		// Controls
		public DelegateCommand CalculateCommand { get; private set; }

		public MainWindowViewModel()
		{
			_title = "Многомерная оптимизация методом Марквардта";

			_output = "Здесь будут выведены результаты работы по оптимизации скалярной функции многих переменных";
			_startPoint = new double[] {0,0,0};
			_maxIter = 200;
			_eps = 0.001;

			CalculateCommand = new DelegateCommand(Calculate);
			_model = new MSOptimizationModel();
		}

		private void UpdateModel()
        {
			_model.Eps = _eps;
			_model.InitValue = _startPoint;
			_model.MaximumIterations = _maxIter;
			_model.Function = new RosenbrockFunction();
        }

		public void Calculate()
		{
			UpdateModel();
			OptimizationResult res = _model.Optimize();
			Output = $"{res.Point}\n{res.Value}\n{res.Iterations}";
		}
	}
}
