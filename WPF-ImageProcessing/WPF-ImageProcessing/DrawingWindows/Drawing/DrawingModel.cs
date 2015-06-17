﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using WPF_ImageProcessing.DrawingWindows.Drawing.DrawingCommand;
using WPF_ImageProcessing.DrawingWindows.Drawing.DrawingState;

namespace WPF_ImageProcessing.DrawingWindows.Drawing
{
    //對本Class的期待
    //--
    //擁有畫圖物件
    //擁有控制繪圖狀態(貼圖 鉛筆...)物件 (yet
    //擁有控制巨集功能
    //--
    //
    //預期
    //--
    //畫圖物件 控制畫板繪畫
    //巨集物件 記錄每次的動作 並可進行返回
    //繪圖狀態物件 可以切換操作模式 並配合 巨集物件 以便紀錄動作
    //--
    //
    //目前
    //--
    //畫圖物件 ..
    //巨集物件 ..
    //繪圖狀態物件 none
    //--
    public class DrawingModel
    {
        private DrawingControl _drawingControl;
        private DrawingCommander _commander;
        private Point _firstPoint;
        private Point _movePoint;
        private Point _finalPoint;
        private bool _Clicked;
        private DrawingStateController _stateController; 
        private List<DrawingAction> _drawingActions; //儲存所有繪圖表上的動作已便於重繪時使用

        private DrawingTools _mode;

        /// <summary>
        /// DrawingWindows' Model
        /// </summary>
        /// <param name="drawingControl">傳入繪圖物件</param>
        public DrawingModel(DrawingControl drawingControl)
        {
            this._drawingControl = drawingControl;
            Initialize();
        }

        public void Initialize()
        {
            _drawingActions = new List<DrawingAction>();
            _commander = new DrawingCommander(this);
            _stateController = new DrawingStateController(this,_commander);
        }

        public void ChangeMode(DrawingTools mode)
        {
            this._mode = mode;
            _stateController.ChangeMode(mode);
            /*_Clicked = false;*/
            //State.Change(_mode);
        }

        public void SetPointDown(double x,double y)
        {
            /*_firstPoint = new Point(x, y);
            _Clicked = true;*/
            //State.PointDown(_firstPoint);
            _stateController.PointDown(new Point(x, y));
        }

        public void SetPointMove(double x, double y)
        {
            /*if (_Clicked == false || _mode == DrawingTools.None)
                return;
           // Console.WriteLine("Move at {0:F} {1:F}", x, y);
            Refresh();
            _movePoint = new Point(x, y);
            DrawingAction act = MakeAction(this._mode, _firstPoint, _movePoint);
            Draw(act);*/
            //State.PointMove(_firstPoint,_movePoint)
            _stateController.PointMove(new Point(x, y));
        }

        public void SetPointUp(double x, double y)
        {
            /*_Clicked = false;
            _finalPoint = new Point(x, y);
            DrawingAction act = MakeAction(this._mode, _firstPoint, _finalPoint);
            EnterCommander(act.Mode, act);
            Refresh();*/
            //State.PointUp(_firstPoint,_movePoint)
            _stateController.PointUp(new Point(x, y));
        }

        public DrawingAction MakeAction(DrawingTools mode,Point leftTop,Point rightBottom)
        {
            DrawingAction act = new DrawingAction(DrawingTools.None, _firstPoint, _finalPoint);
            if(mode == DrawingTools.Pencil)
                act = new DrawingAction(mode, leftTop, rightBottom);
            return act;
        }

        /*private void EnterCommander(DrawingTools mode,DrawingAction act)
        {
            if (mode == DrawingTools.Pencil)
                _commander.Execute(new PencilCommand(this, act));
        }*/


        public void Refresh()
        {
            _drawingControl.ClearAll();
            if(_drawingActions.Count > 0)
                foreach (DrawingAction act in _drawingActions)
                    Draw(act);
        }

        public void Undo()
        {
            _commander.Undo();
            //State.Undo();
        }

        public void Redo()
        {
            _commander.Redo();
            //State.Redo();
        }

        public void Draw(DrawingAction act)//TemporaryDrawing
        {
            if (act.Mode == DrawingTools.Pencil)
                _drawingControl.DrawLine(act.LeftTop.X, act.LeftTop.Y, act.RightBottom.X, act.RightBottom.Y);
            //State.Draw();
        }

        public void TakeOfAction(DrawingAction act)
        {
            if (!_drawingActions.Remove(act))
                throw new KeyNotFoundException("Drawing Action in Drawing Model");
        }

        public void AddAnAction(DrawingAction act)
        {
            _drawingActions.Add(act);
        }
    }
}
