namespace MSOptimization.ViewModels
{
	using MSOptimization.Models;
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
			set => SetProperty(ref _startPoint, Array.ConvertAll(value.Split(" "), Double.Parse));
		}

		public string MaxIterations
		{
			get => _maxIter.ToString();
			set => SetProperty(ref _maxIter, Int32.Parse(value));
		}

		public string Eps
		{
			get => _eps.ToString(_cultureInfo);
			set => SetProperty(ref _eps, Double.Parse(value, _cultureInfo));
		}

		// Controls
		private readonly DelegateCommand CalculateCommand;

		public MainWindowViewModel()
		{
			_title = "";

			_output = "Здесь будут выведены результаты работы по оптимизации скалярной функции многих переменных";
			_startPoint = new double[] {0,0,0};
			_maxIter = 200;
			_eps = 0.001;

			CalculateCommand = new DelegateCommand(Calculate);
			_model = new MSOptimizationModel(_startPoint,_eps,_maxIter);
		}

		public void Calculate()
		{

		}
	}
}
