   M S B A M L   `   `   `  ����G  CVCheckViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:VCheckViewer.Views.Pages.Notification%VCheckViewer.Views.Pages.Notification  5      E'clr-namespace:VCheckViewer.UserControlsVCheckViewer.UserControls  6{   <    6VCheckViewer.Views.Pages.Notification.NotificationPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Elocal3clr-namespace:VCheckViewer.Views.Pages.Notification     5      A =Wpf.Ui, Version=3.0.4.0, Culture=neutral, PublicKeyToken=null/ui&http://schemas.lepo.co/wpfui/2022/xaml  6I   :userControls'clr-namespace:VCheckViewer.UserControls  6{     N� Title$  NotificationPage��5       � 	Resources 5
         test �� 
TargetType �� Property �� Value��%R   &	        �� 5   
   " ]�6   .�5      ��  � ��$ 80��6'   5      5      .��5      � -   5        $��	0,10,0,10q�5       � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��0.65* �6   �� 5      $��0.84* �6   �� 5      $
��0.7* �6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��6* �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��4��6   
������� �� CornerRadius$ 40��6J   $��	10,5,10,5q�6\   6q   �� 5   
   $��Stretch=�6   $��Center=�6/   $��0��6J   $��	20,0,10,0q�6Z   ._�5      �� -   5        
�������$ 20��5   (    �� Padding$ 5q�6:   $��1q�6F   
���   �._�5      � .+�5!         Wpf.Ui.Controls.SymbolIcon  	   Symbol$	 Search12=�6$     Wpf.Ui.Controls.FontIcon
   FontSize$
 20(�66   $
��Left=�6D   $��5,2,0,0q�6_   �� 5$      -   5         ��Name$ KeywordSearchBar��5$      $��0q�68   $��	30,0,10,0q�6L   $��Center=�6_   6z   �� 5%       J� IsHitTestVisible$ False��6    $��Center=�6�   $
��Left=�6�   $��	40,0,10,0q�6�   
���������69   ��  �� Path$ Results_Label_Searchbar�  	Resources �� Source# [ ��5&      �� 5'      " ��6$   .�5(   "   ��  � y�$ 	Collapsed=�6?    �� Triggers 5)   "   �� 5*   &    �� Binding 62   �� *Text+ �� ElementName$ KeywordSearchBar�� �� Value  6i   .a�5+   *   ��  � y�$ Visible=�6G   5,   '   5.      50      51      52      53      �� 54   
   $��Stretch=�6   $��Center=�6/   $��1��6J   $��0,0,0,0q�6Z   ._�55      �� 
�������$ 20��6(   $ 5q�6:   $��1q�6F   
���   �._�56      � .+�57        $	 CalendarLtr32=�6$   $
 20(�6;   $
��Left=�6I   $��5,2,0,0q�6d   /   )VCheckViewer.UserControls.DateRangePicker  5:      -   5          Name$ 	RangeDate��5:   3   $��0q�6�   $��	30,0,10,0q�6�   $��Center=�6�      SelectedDateRange 6F   �� *SelectedDates+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   5;      5<      5=      �� 5>   
   $��2��6   $��45��6!   $ 20��6-   $��1q�6?   
���   �
���   �$��10,0,0,0q�6z   ._�5S      �� 
�������$ 20��6(   $ 5,0,5,0q�6:   ._�5T      � .+�5V      �� $��Show :��6    $��15(�6.   $
��Left=�6<   $��Center=�6W   $��5,0,0,0q�6r   �� 5X      -   5        $��NotificationType��5X      
�������$��0q�6J   $��Center=�6^   $��50,0,0,0q�6y   $
��Bold"�6�   .@�5Y      �� $��Normal"�6~    �� 
IsSelected$
 True��6�   ��6'   �� $  Notification_Label_ShowAll�# [ 6�   �� 5Z      
 � Tag$ Upcoming Test��6�   $��Normal"�6�   ��6'   �� $% Notification_Label_UpcomingTest�# [ 6�   �� 5[      $ Completed Test Results��6�   $��Normal"�6�   ��6'   �� $&  Notification_Label_CompletedTest�# [ 6�   �� 5\      $ Updates��6~   $��Normal"�6�   ��6'   �� $  Notification_Label_Updates�# [ 6�   5]      5^      5_      5`      �� 5a   
   $��Stretch=�6   $��Center=�6/   $��3��6J   $��	10,0,20,0q�6Z   ._�5b      �� -   5        
�������$��Stretch=�5b   �   
��� ���$��0q�6�     RoundButton#���  6�   ��6   �� $% Notification_Label_UpdateFilter�# [ 6�   5c      �� 5d   
   $��4��6   $��1��6%   
�������$ 20��6J   $��10q�6\   $ 70,20,70,30q�6h   ._�5e      �� -   5         ��Name$ NotificationViewList��5e      
������ 5�      5�      -   'VCheckViewer.UserControls.PagingControl  5�   
   -   5          Name$ 
pagination��5�   %   $��2��69   $��0��6F   $��6��6V   5�      5�      5�      