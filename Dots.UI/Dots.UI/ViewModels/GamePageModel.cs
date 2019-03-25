using Dots.Core.Field.Models;
using Dots.Core.Game;
using Dots.UI.Models;
using FreshMvvm;
using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Dots.UI.ViewModels
{
    public class GamePageModel : FreshBasePageModel
    {
        private Field _field;
        private Game _game;
        private string _player;
        private Color _playerColor;
        private string _score;

        public Field Field
        {
            get => _field;

            set
            {
                _field = value;
                RaisePropertyChanged();
            }
        }

        public string Player
        {
            get => _player;

            set
            {
                _player = value;
                RaisePropertyChanged();
            }
        }

        public Color PlayerColor
        {
            get => _playerColor;

            set
            {
                _playerColor = value;
                RaisePropertyChanged();
            }
        }

        public ICommand ResetCommand { get; set; }

        public string Score
        {
            get => _score;

            set
            {
                _score = value;
                RaisePropertyChanged();
            }
        }

        public ICommand TappedCommand { get; set; }

        public override void Init(object initData)
        {
            base.Init(initData);

            ResetCommand = new Command(CreateNewGame);

            TappedCommand = new Command(dotModel =>
            {
                if (dotModel is DotModel model)
                    try
                    {
                        _game?.MakeMove(model.Row, model.Column);

                        if (_game != null)
                        {
                            Player = _game.FirstPlayerMove ? "First player" : "Second player";
                            PlayerColor = _game.FirstPlayerMove ? Color.Blue : Color.Brown;
                            Score = $"Score: {_game.Result.FirstPlayerScore} : {_game.Result.SecondPlayerScore}";
                        }
                    }
                    catch (Exception exception)
                    {
                        CurrentPage.DisplayAlert("Error", exception.Message, "OK");

                        var properties = new Dictionary<string, string>
                        {
                            ["Message"] = exception.Message,
                            ["StackTrace"] = exception.StackTrace
                        };
                        Analytics.TrackEvent("Error", properties);
                    }
            });

            CreateNewGame();
        }

        private void CreateNewGame()
        {
            if (_game != null)
                _game.OnFieldChanged -= OnFieldChanged;

            _game = new Game();
            _game.OnFieldChanged += OnFieldChanged;
            _game.Initialyze(10);

            Player = _game.FirstPlayerMove ? "First player" : "Second player";
            PlayerColor = _game.FirstPlayerMove ? Color.Blue : Color.Brown;
            Score = $"Score: {_game.Result.FirstPlayerScore} : {_game.Result.SecondPlayerScore}";
        }

        private void OnFieldChanged(Field field)
        {
            Field = field;
        }
    }
}