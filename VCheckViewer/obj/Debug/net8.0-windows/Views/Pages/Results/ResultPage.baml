   M S B A M L   `   `   `  ����G  CVCheckViewer, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullS.clr-namespace:VCheckViewer.Views.Pages.Results VCheckViewer.Views.Pages.Results  5      E'clr-namespace:VCheckViewer.UserControlsVCheckViewer.UserControls  5      1    +VCheckViewer.Views.Pages.Results.ResultPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      @local.clr-namespace:VCheckViewer.Views.Pages.Results     5      A =Wpf.Ui, Version=3.0.5.0, Culture=neutral, PublicKeyToken=null/ui&http://schemas.lepo.co/wpfui/2022/xaml  5      :userControls'clr-namespace:VCheckViewer.UserControls  5        N� Title$  
ResultPage��5       � 	Resources 5         DataGridVerticalText �� 
TargetType �� Property �� Value  Themes_GridResultFontColor % !DataGridVerticalTextCenterAligned  TextBoxStyle   Themes_SearchFieldBackground  Themes_SearchFieldFontColor  ColumnHeaderStyle1=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType  Themes_GridHeaderFontColor   columnHeaderStyleCenterAlign  	 BooleanToVisibilityConverter��%2  &	        &	 ,    &	 �    &	 �    &	 �    &		 �    �� 5   
   " ��6-   .�5      ��  � ��$ Center=�62   �� 5       � ��$
 Left=�64   �� 5       � ��$ 	Noto Sans)�6+   �� 5       � ��$ 12(�6)   �� 5       � ��# �  6+   �� 5       � ��$ 2q�6'   �� 5       � ��$
 Wrap=�67   5      �� 5   
   " ��6:   .�5      ��  � ��$ Center=�62   �� 5       � ��$
 Left=�64   �� 5       � ��$ 	Noto Sans)�6+   �� 5       � ��$
 Wrap=�67   5      �� 5   
   " ��6%   .�5       ��  � ��# �  6+   �� 5!       � ��$ Center=�69   �� 5"       � ��# �  6+   �� 5#       � ��$ 
5, 2, 0, 0q�6(   5$      �� 5'   
   "  5(      .�5*      ��  � �� 5+      �� 5,      "  6&   .R�5-      �� 
�������
������ $��0q�5/      ._�51      �� #��z��6(   #���  52   (   $
��Left=�53   (   $
��Bold"�54   (   $��13(�55   (   57      58      5:      5;      �� 5<   
   "  65   .�5=      ��  � �� 5>      �� 5?      "  6&   .R�5@      �� 
�������
������ $��0q�5B      ._�5D      �� #��z��6(   #���  5E   (   $��Center=�5F   (   $
��Bold"�5G   (   $��13(�5H   (   5J      5K      5M      5N      �� 5P   
   6'   5Q      .��5S      � -   5        $��0, 10, 0, 10q�5S       � ColumnDefinitions 5T   
   �� 5U      $��* �6   �� 5V      $��4* �6   �� 5W      $��4* �6   �� 5X      $��4* �6   �� 5Y      $��3* �6   �� 5Z      $��3* �6    � RowDefinitions 5\   
   �� 5]      $
��0.5* �6   �� 5^      $��7* �6   �� 5_      $��* �6   .+�5b   
   �� -   5        
���   �$��0q�5b   %   $��0��69   $��6��6I   $��0��6]   ._�5c      � $��20, 5, 0, 5q�6    5d      �� 5e      $	��15* �6'   �� 5f      $��* �6'   �� 5g      $��* �6'   .+�5j      �� 
���   �$��0q�6-   $��0��6A   $��0��6Q    �� CornerRadius$ 7��6^   
������ ._�5k      � $��
5, 2, 5, 2q�6    5l      �� 5m      $��4* �6/   �� 5n      $��4* �6/   �� 5o      $��3* �6/   �� 5p      $��2* �6/   .+�5s      �� 
���   �$��0q�65   $��0��6I   ._�5t      �� #���  6%   $��1q�6b   #���  6v   $ 7��6�   $��10, 5, 5, 5q�6�   ._�5u   "   � .+�5w   &   ��  
 Themes_SearchIcon#��� 
 6,   $��24��6Y   $
��Left=�6d   $��
5, 2, 0, 0q�6   �� 5y   &   -   5        	 ��Name$	 KeywordSearchBar��5y   .   $��0q�6H   $��	30,0,10,0q�6\   $��Center=�6o   #��[ 6�   6�   �� 5z   &   
 J� IsHitTestVisible$
 False��60   $��Center=�5{   0   $
��Left=�6K   $��	40,0,10,0q�6f   
�������$��	Noto Sans)�5|   0   $��12(�6G   #���  6U   ��5z   I   ��  �� Path$ Results_Label_Searchbar�  	Resources �� Source# [ ��5}   *   �� 5~   .   " ��64   .�5   2   ��  � y�$ 	Collapsed=�6O    �� Triggers 5�   2   �� 5�   6    �� Binding 6B   �� *Text+ �� ElementName$ KeywordSearchBar�� �� Value  6y   .a�5�   :   ��  � y�$ Visible=�6W   5�   7   5�   /   5�   '   5�   #   5�      5�      �� 5�      
���   �$��0q�65   $��1��6I   
������ ._�5�      �� #���  6%   $ 7��6a   $��
5, 5, 5, 5q�6r   $��1q�6�   #���  6�   ._�5�   "   � .+�5�   &   ��   Themes_CalendarIcon#���  6,   $��24��6[   $
��Left=�6f   $��5,2,0,0q�6�   /   )VCheckViewer.UserControls.DateRangePicker  5�   &   -   5          Name$ 	RangeDate��5�   C   $��0q�5�   5   $��	30,0,10,0q�6I   $��Center=�6\   $��	Noto Sans)�5�   5   $��12(�6L      SelectedDateRange 5�   5   �� *SelectedDates+ �� Mode$ TwoWay=�5�   L   5�   #   5�      5�      �� 5�      
���   �$��0q�65   $��2��6I   ._�5�      �� $ 7��6%   $��
5, 5, 5, 5q�66   $��1q�6J   #���  6^   #���  6�   ._�5�   "   � .+�5�   &   �� $��	Noto Sans)�5�   ,   #���  6C   $��12(�6~   $��Center=�6�   $��
5, 5, 0, 0q�5�   ,   ��5�   ,   �� $ Results_Label_SortBy�# [ 5�   ,   �� 5�   &   -   5         ��Name$ cboSort��5�   /   
������ $��0q�6Y   $��60, 0, 5, 0q�6m   #���  5�   r   ��6+   �� *cbSort+ �� SelectedItem 6J   �� *SelectedcbSort+6�   5�   #   5�      5�      �� 5�      
���   �$��0q�65   $��3��6I   ._�5�      �� $ 7��6%   $��5q�66   $��0q�6A   
���   �  Themes_ButtonColor#���  6i   ._�5�   "   �� -   5        $��	btnFilter��5�   )   $	��120��5�   )   
�������$��Stretch=�6H   $��Center=�6d   $��0q�5�   )   
������   RoundButton#���  5�   )   ��5�   :   �� $  Results_Label_UpdateFilter�# [ 5�   #   5�      5�      5�      5�      �� 5�      
���   �$��0q�6-   $��1��6A   $��0��6Q   $��Right=�6^   ._�5�      �� -   5        $��btnDownload��5�      
���   �$��0q�6D   
������ .��5�      ��   Themes_GridDownloadIcon#���  6    $��20��6S   $��20��6^   5�      5�      �� 5�      
���   �$��0q�6-   $��2��6A   $��0��6Q   $��Center=�6^   ._�5�      �� -   5        $��btnPrint��5�      $��
1, 0, 5, 0q�6-   $��0q�6A   
���   �
������ .��5�      ��   Themes_GridPrintIcon#���  6    $��20��6P   $��20��6[   5�      5�      5�      5�      �� 5�   
   -	   5        $��txtTotalCount��5�      $��0��6)   $y�	Collapsed=�62   � 5�   
   $��1��6   $��0��6   $��6��6,   $��15, 10, 10, 5q�6@   .+�5�      &   System.Windows.Controls.DataGrid  -
   5        $��dgResult��5�      $��0q�6'      AutoGenerateColumns$ False��6;      GridLinesVisibility$ 
Horizontal=�6W   
������    RowHeaderWidth$ 0Q�5�         ColumnHeaderStyle# �  6$     Themes_GridRowLinesColor   HorizontalGridLinesBrush# �  6]      CanUserAddRows$ False��6�      Columns 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ 35 6�   1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [  6�      Header 6)   �� $ Results_Label_No�# [     Binding  6u   �� *RowNo+6�     5�      $ * 6�   # [  6�    6)   �� $ Results_Label_PatientName�# [   6~   �� *PatientName+6�     5�      $ * 6�   # [  6�    6)   �� $ Results_Label_OperatorID�# [   6}   �� *
OperatorID+6�     5�      $ * 6�   # [  6�    6)   �� $ Results_Label_PatientID�# [   6|   �� *	PatientID+6�     5�      $ * 6�   # [  6�    6)   �� $" Results_Label_TestResultType�# [   6�   �� *TestResultType+6�   4  .System.Windows.Controls.DataGridTemplateColumn  5�      $ * 6}    6-   �� $ Results_Label_Result�# [ !   CellTemplate! 5�      �� 5�      .R�5�   "   �� $ 4��6)   $��0, 5, 15, 5q�6:   
������ ._�5�   &   �� $��11(�6R   $
��Bold"�6`   $��Center=�5�   W   $
��Left=�5�   0   $��WrapWithOverflow=�6K   ��5�   0   �� *TestResultStatus+��5�   0   �� *statusFontColor+5�   K   5�   #   5�      5�        5�      $ * 6�   # [  6�    6)   �� $ Results_Label_Time�# [   6w   �� *TestResultDateTime+" �� StringFormat$" {0:yyyy-MM-dd HH:ss}��6�     5�      $ * 6�   # [  6�    6)   �� $ Results_Label_Doctor�# [   6y   �� *InchargePerson+6�     5�      $ * 6�   # [  6�    6)   �� $" Results_Label_DeviceSerialNo�# [   6�   �� *DeviceSerialNo+6�     5�      $
 0.3* 6-   ! 5�      �� 5�      .R�5�   "   � $��Right=�6'   .+�5�   &   o� .@�5�   *   m� # m� Icon# 5�   .   �� 5�   2     Themes_GirdUserMoreIcon#���  68   $��12��6k   .@�5�   .   m� $��menuEditPatientName��67   -   5        
$ � Tag$ 5�   R   �� *ID+��6e   �� $# Results_Label_EditPatientName�# [ 6�   m� 5�   .   $��menuView��67   -   5        ��5�   G   �� $ Results_Label_View�# [ 6�   m� 5�   .   $��menuDownload��67   -   5        ��5�   K   �� $ Results_Label_Download�# [ 6�   m� 5�   .   $��	menuPrint��67   -   5        ��5�   H   �� $ Results_Label_Print�# [ 6�   m� 5�   .   $��menuTransfer��67   -   5        $ 5�   K   �� *ID+��6^   �� $ Results_Label_SendToPMS�# [ y�6�   �� *PMSFunction+5�   +   5�   '   5�   #   5�      5�      5�      �� 5�      % �� TextAlignment$% Center=�6h   $��0,50,0,0q�6     Themes_FontColor#���  6  ��6   �� $ General_Message_NoData�# [ y�6�   �� *Items.IsEmpty+& �� 	Converter#& [	 $ dgResult��6  5�      -	   'VCheckViewer.UserControls.PagingControl	  5�   
   -   5        ' 	 Name$' 
pagination��5�   %   $��2��69   $��0��6F   $��6��6V   5�      5      5     