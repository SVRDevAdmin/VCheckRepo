   M S B A M L   `   `   `  ����G  CVCheckViewer, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullC&clr-namespace:VCheckViewer.Views.PagesVCheckViewer.Views.Pages  5      E'clr-namespace:VCheckViewer.UserControlsVCheckViewer.UserControls  5      ?$clr-namespace:VCheckViewer.ConverterVCheckViewer.Converter  5   9   '    !VCheckViewer.Views.Pages.UserPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:VCheckViewer.Views.Pages     5      @userControls'clr-namespace:VCheckViewer.UserControls     5      A =Wpf.Ui, Version=3.0.5.0, Culture=neutral, PublicKeyToken=null/ui&http://schemas.lepo.co/wpfui/2022/xaml  5      4	Converter$clr-namespace:VCheckViewer.Converter  69     N� Title$  UserPage��5
       � 	Resources 5          BooleanToVisibilityConverter-   'VCheckViewer.Converter.IndexedConverter  IndexedConverter # DataGridVerticalTextLeftAligned �� 
TargetType �� Property �� Value  Themes_GridUserFontColor % !DataGridVerticalTextCenterAligned  ColumnHeaderStyle1=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType " Themes_GridUserHeaderFontColor��%w  &	        &	      &	 &     &	 .    &	 8    �� 5   
   6'     5   
   6%   �� 5   
   " ��68   .�5      ��  � ��$ Center=�62   �� 5       � ��$
 Left=�64   �� 5       � ��$ 	Noto Sans)�6+   �� 5       � ��$
 Wrap=�67   �� 5       � ��# �  6+   �� 5       � ��$ 5q�6'   5      �� 5   
   " ��6:   .�5      ��  � ��$ Center=�62   �� 5       � ��$ Center=�64   �� 5       � ��$ 	Noto Sans)�6+   �� 5       � ��$
 Wrap=�67   �� 5       � ��# �  6+   �� 5       � ��$ 5q�6'   5      �� 5"   
   "  5#      .�5%      ��  � �� 5'      �� 5)      "  6&   .R�5+      �� 
������ 
������ $��5q�5-      ._�5/      �� #��z��6(   #���  50   (   $��Center=�51   (   $
��Wrap=�52   (   54      56      5:      5<      5=      .��5?      � $��	0,10,0,10q�6    � RowDefinitions 5@   
   �� 5A      $
��auto �6   �� 5B      $��* �6    � ColumnDefinitions 5D   
   �� 5E      $��* �6   �� 5F      $��2* �6   �� 5G      $��2* �6   �� 5H      $��* �6   .+�5K   
   �� $��1��6   $��2��6!   $��45��65    �� CornerRadius$ 7��6A   $��1q�6R   
���   �
���   �$	��500��6�   ._�5L      �  5M      �� 5N      $
��auto �6'   $��90��64   �� 5O      $��* �6'   �� 5P      $
��auto �6'   $��90��64   �� 5Q      $
��auto �6'   $��90��64   �� 5R      $
��auto �6'   $��90��64   .+�5T      �� $��0��6   $��2q�6)   ._�5U      �� 
�������$��Stretch=�6     Themes_ButtonColor#���  6�   $��0q�6�     RoundButton#���  6�   ��6   �� 	 �� Path$	 Setting_Label_User� 	 	Resources
 �� Source#
 [	 6  5V      �� 5W      $��1��6   $��2q�6)   ._�5X      �� -   5        
�������$��Stretch=�5X   �   $��0q�6�   
���   �#���  6�   ��6   �� $#	 Setting_Label_LanguageCountry�#
 [	 6  5Y      �� 5Z      $��2��6   $��2q�6)   ._�5[      �� -   5        $��btnSettings��5[      $��PMS/LIS/HIS��60   
�������$��Stretch=�6Y   $��0q�6w   
���   �#���  6�   6�   5\      �� 5]      $��3��6   $��2q�6)   ._�5^      �� -   5        $��	btnDevice��5^      
�������$��Stretch=�6�   $��0q�6�   
���   �#���  6�   ��6.   �� $	 Setting_Label_Device�#
 [	 6  5_      �� 5`      $��4��6   $��2q�6)   ._�5a      �� -   5        $��	btnReport��5a   m   
�������$��Stretch=�6�   $��0q�6�   
���   �#���  6�   ��6   �� $	 Report_Label_Report�#
 [	 6  5b      5c      5d      �� 5f   
   -   5        $	��Top=�5f   f   $��3��6�   $��0��6�   $��Stretch=�6�   $��45��6�   #���  6�   #���  6  $��5,0,20,0q�67  #���  6I   
 Themes_ButtonFontColor#��� 
 6|  ��6   �� $	 Setting_Label_CreateUser�#
 [	 6�  �� 5g   
   $��1��6   $��4��6   $��0,20,0,0q�62   ._�5h      �  5i      �� 5j      $��* �6$   �� 5k      $
��auto �6$   .+�5m      &   System.Windows.Controls.DataGrid  -   5        $��dataGrid��5m         
IsReadOnly$
 True��6-   
������ 
���   �   AutoGenerateColumns$ False��6l   $��10q�6�   $��0��6�      GridLinesVisibility$ 
Horizontal=�5n      $��0q�6<      RowHeaderWidth$ 0Q�6Q      ColumnHeaderStyle# �  6d      Themes_GridUserRowLinesColor   HorizontalGridLinesBrush# �  5o       �� AlternationCount 5p      �� *Items.Count+ �� RelativeSource �� *Self+   Columns 5q      0  *System.Windows.Controls.DataGridTextColumn  5s      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 auto 6  1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [ 6     Header 6-   �� $	 General_Label_Number�#
 [	    Binding 6   �� *AlternationIndex+ ��  �� AncestorType DataGridRow �� 	Converter# [ 6    5t      $
 1.1* 6�      MinWidth$	 180Q�6�   # [ 6�      
FontWeight$
 Bold"�6   6-   �� $	 Setting_Label_StaffName�#
 [	  6�   �� *	StaffName+6    5u      $
 0.5* 6�   # [ 6�    6-   �� $	 Setting_Label_StaffID�#
 [	  6�   �� *
EmployeeID+6�     5v      $	 200Q�6�   # [ 6�    6-   �� $"	 Setting_Label_RegistrationNo�#
 [	  6�   �� *RegistrationNo+6�     5w      $
 0.5* 6�   # [ 6�      
Visibility$ 	Collapsed=�6�    6-   �� $	 Setting_Label_Gender�#
 [	  6   �� *Gender+6�     5x      $
 0.6* 6�   # [ 6�    6-   �� $	 Setting_Label_Role�#
 [	  6}   �� *Role+6�     5y      $	 110Q�6�   # [ 6�   $ 	Collapsed=�6�    6-   �� $	 Setting_Label_DateOfBirth�#
 [	  6�   �� *DateOfBirth+6�     5z      $	 220Q�6�   # [ 6�    6-   �� $	 Setting_Label_Email�#
 [	  6~   �� *EmailAddress+6�   4  .System.Windows.Controls.DataGridTemplateColumn  5{      $
 auto 61      CellTemplate 5|      �� 5}   "   .R�5~   &   � $��Right=�6+   .+�5   *   o� .@�5�   .   m�  m� Icon 5�   2   �� 5�   6     Themes_GirdUserMoreIcon#���  6<   $��12��6o   .@�5�   2   m� -   5        ��5�   ;   �� $	 General_Label_View�#
 [	 6�   m� 5�   2   -   5        ��5�   ;   �� $	 General_Label_Update�#
 [	 6�   5�   /   5�   +   5�   '   5�   #   5�      5�      �� 5�       �� TextAlignment$ Center=�6p   $��0,50,0,0q�6�     Themes_FontColor#���  6  ��6   �� $ 	 Setting_Message_NoUserData�#
 [	 y�6�   �� *Items.IsEmpty+# [    �� ElementName$  dataGrid��6  -	   'VCheckViewer.UserControls.PagingControl	  5�      -	   5        ! 	 Name$! 
pagination��5�   -   $��1��6A   $��Right=�6N   5�      5�      5�      5�      5�      