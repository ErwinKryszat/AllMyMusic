using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls.Primitives;




namespace AllMyMusic_v3.View
{
    public class ListBoxSongItems : ListBox
    {
        #region SelectedItemsList
        public static readonly DependencyProperty SelectedItemsListProperty = DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(ListBoxSongItems), new PropertyMetadata(null));
        public IList SelectedItemsList
        {
            get { return (IList)GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }
        #endregion

       

        public static readonly DependencyProperty InsertSongsIndexProperty = DependencyProperty.Register("InsertSongsIndex", typeof(int), typeof(ListBoxSongItems), new PropertyMetadata(-1));
        public int InsertSongsIndex
        {
            get { return (int)GetValue(InsertSongsIndexProperty); }
            set { SetValue(InsertSongsIndexProperty, value); }
        }



        public ListBoxSongItems()
        {
            this.SelectionChanged += ListBoxSongItems_SelectionChanged;
            MyDefaults();
        }

        private void ListBoxSongItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItemsList = this.SelectedItems;

        }

        private void MyDefaults()
        { 
            //this.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
            //this.AllowDrop = true;
        }

        #region Drag & Drop Rows

        
        //On MouseDown with left button down, record the position (on MouseLeave erase the position)
        //On MouseMove with {left button down, position recorded, current mouse position differs by more than delta} set a flag saying drag operation is in progress & capture the mouse
        //On MouseMove with drag operation in progress, use hit testing to determine where your panel should be (ignoring the panel itself) and adjust its parenting and position accordingly.
        //On MouseUp with drag operation in progress, release the mouse capture and clear the "drag operation is in progress" flag



        //public delegate Point GetPosition(IInputElement element);
        //int rowIndex = -1;
        private Point mouseDownPosition;
        private Boolean dragDropInProgress = false;
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            mouseDownPosition = e.GetPosition(this);
            dragDropInProgress = false;
            this.Cursor = Cursors.Arrow;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            mouseDownPosition = new Point();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if ((e.LeftButton == MouseButtonState.Pressed) && (mouseDownPosition.X > 0))
            {
                Point newPosition = e.GetPosition(this);
                if (Math.Abs(newPosition.Y - mouseDownPosition.Y) > 10)
                {
                    dragDropInProgress = true;
                    InsertSongsIndex = -1;
                    this.Cursor = Cursors.Hand;
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                    mouseDownPosition = newPosition;
                }
            }
        }

        public delegate Point GetPosition(IInputElement element);

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (dragDropInProgress == true)
            {

                int insertIndex = this.GetCurrentRowIndex(e.GetPosition);
                if (insertIndex < 0)
                    return;

                if (insertIndex == this.Items.Count - 1)
                {
                    MessageBox.Show("This row-index cannot be drop");
                    return;
                }

                InsertSongsIndex = insertIndex;
            }

            this.Cursor = Cursors.Arrow;
        }


        private bool GetMouseTargetRow(Visual theTarget, GetPosition position)
        {
            if (theTarget == null)
            {
                return false;
            }
            Rect rect = VisualTreeHelper.GetDescendantBounds(theTarget);
            Point point = position((IInputElement)theTarget);
            return rect.Contains(point);
        }

        private ListBoxItem GetRowItem(int index)
        {
            if (this.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;
            return this.ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem;
        }

        private int GetCurrentRowIndex(GetPosition pos)
        {
            int curIndex = -1;
            for (int i = 0; i < this.Items.Count; i++)
            {
                ListBoxItem itm = GetRowItem(i);
                if (GetMouseTargetRow(itm, pos))
                {
                    curIndex = i;
                    break;
                }
            }
            return curIndex;
        }
        #endregion


       
    }
}
