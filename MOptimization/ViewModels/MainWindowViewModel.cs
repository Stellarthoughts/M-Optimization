namespace MSOptimization.ViewModels
{
    using MSOptimization.Core;
    using MSOptimization.Models;
    using MSOptimization.NumericMethods;
    using Prism.Commands;
	using Prism.Mvvm;
	using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Controls;

	public class MainWindowViewModel : BindableBase
	{
		// Window settings
		private string _title = "Многомерная оптимизация методом Марквардта";

		public string Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}

		// Input settings
		private CultureInfo _cultureInfo = CultureInfo.InvariantCulture;

		// Optimization
		private string _output = "Здесь будут выведены результаты работы по оптимизации скалярной функции многих переменных";
		private double[] _startPoint = new double[] { 0, 0 };
		private int _maxIter = 200;
		private double _eps = 0.001;
		private MSOptimizationModel _model;
		private MSFunction _function;

		// Checkbox
		private bool _isCheckedSpherical1;
		private bool _isCheckedRosenbrock;
		private bool _isCheckedPowell;

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

		// RBs
		public bool IsCheckedSpherical1
		{
			get => _isCheckedSpherical1;
			set
			{
				if (SetProperty(ref _isCheckedSpherical1, value))
					if(value) _function = new SphereFunction1();
			}
		}
		public bool IsCheckedRosenbrock
		{
			get => _isCheckedRosenbrock;
			set
			{
				if (SetProperty(ref _isCheckedRosenbrock, value))
					if (value) _function = new RosenbrockFunction();
			}
		}
		public bool IsCheckedPowell
		{
			get => _isCheckedPowell;
			set
			{
				if (SetProperty(ref _isCheckedPowell, value))
					if (value) _function = new PowellFunction();
			}
		}

		// Commands

		public DelegateCommand CalculateCommand { get; private set; }

		// Constructor
		public MainWindowViewModel()
		{
			CalculateCommand = new DelegateCommand(Calculate);
			IsCheckedSpherical1 = true;

			_model = new MSOptimizationModel();
		}

		// Public functions
		public void Calculate()
		{
			UpdateModel();
			OptimizationResult res = _model.Optimize();
			Output =
				$"Точка: {string.Join(" ", res.Point)}\n" +
				$"Значение: {res.Value}\n" +
				$"Количество итераций: {res.IterationsCount}\n" +
				$"Точность достигнута: {(res.IsAccuracyAchived ? "Да" : "Нет. Увеличьте количество итераций")}";
		}

		// Private functions
		private void UpdateModel()
        {
			_model.Function = _function;
			_model.Eps = _eps;
			_model.MaximumIterations = _maxIter;

			ValidateStartPoint();
			_model.InitValue = _startPoint;
		}

		private void ValidateStartPoint()
		{
			int argc = _model.Function.ArgCount;
			if (_startPoint.Length != argc)
			{
				double[] formattedArg = new double[argc];
				if (_startPoint.Length > argc)
				{
					Array.Copy(_startPoint, formattedArg, argc);
				}
				else
				{
					Array.Copy(_startPoint, formattedArg, _startPoint.Length);
					Array.Fill(formattedArg, 0, _startPoint.Length, argc - _startPoint.Length);
				}
				_startPoint = formattedArg;
				RaisePropertyChanged(nameof(StartPoint));
			}
		}
	}
}
