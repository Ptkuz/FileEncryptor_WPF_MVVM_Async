   M S B A M L   `   `   `  ����H  DFileEncryptor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:FileEncryptor.Views.WindowsFileEncryptor.Views.Windows  5   	   1    +FileEncryptor.Views.Windows.EncryptorWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7l)clr-namespace:FileEncryptor.Views.Windows     5   	   $	��217��5   &   $	��492��63     � DataContext  5   	   ��  �� Path$ EncryptorWindowModel�   locator �� Source# [   � Title 5   	   �� $ Title�.��5	      ]� .+�5
   
   o� $	��Top=�6   .@�5      m� $��Файл��6   .@�5      m� $��
Выход��6     CloseWindowCommand m� Command# [ 6*   5      5      �� 5   
   $��Bottom=�6   .@�5      �� $��Right=�6   .��5      �� 5      �� 5      $��Right=�6   .��5      ]� ��5      �� 5       �� 
TargetType" ]�6     �� Triggers 5      �� 5   "    �� Binding 6.   �� $ ProgressValue� �� Value 06U   .a�5   &   �� 	 �� Property	 � y�
 �� Value$
 Hidden=�6C   5   #   �� 5   "    6.   �� $ ProgressValue� 16U   .a�5   &   �� 	 � y�$
 Hidden=�6C   5   #   5!      .+�5#      �� $��Right=�6    $��39��67    �� TextAlignment$ Right=�6E   ��6\   �� $ ProgressValue� �� StringFormat$ p1��� 5$      $	��200��6"   $��15��6.    � Maximum$ 1Q�6:    � Value 6F   �� $ ProgressValue�5%      5&      �� 5'      $��Готово!��6   5(      � 5)   
   $��3q�6    � 	Resources 5*       P� Property P� Value��%V  '��       '�� �     �� 5+      " ��6    5,      P� 5-       � ��$ 0��69   . �5.      �� 	 � ��$
 Center=�6B   �� 5/      	 � ��$
 Right=�6D   �� 50      	 � ��$

 Bold"�6;   �� 51      	 � ��$
 0,0,3,0q�67   52      54      �� 59      " ��6   .�5:      �� 	 � ��$
 3q�6/   5;      5<       � RowDefinitions 5=      �� 5>      $
��Auto �6    �� 5?      $
��Auto �6    �� 5@      $
��Auto �6    �� 5A       � ColumnDefinitions 5D      �� 5E      $
��Auto �6#   �� 5F      .+�5H      �� $��0��6   $��0��6%   $��Пароль:��65   �� 5I      $��1��6   $��0��6&   ��63   �� *
Password+ �� UpdateSourceTrigger$ PropertyChanged=��� 5K      $��1��6   $��0��6%   $��	Файл:��65   ]� 5L      $��1��6   $��1��6(   .+�5M      �� $��Right=�6   $��
Выбор��61   $
��10,0q�6A   ��5N      �� *SelectFileCommand+�� 5O       �� 
IsReadOnly$
 True��6   $��0q�6,   ��5P      �� $ SelectedFile.Name� �� Mode$ OneWay=� � ToolTip 5Q      �� $ SelectedFile.FullName�$ OneWay=�5R      �� 5T      $��2��6   $��0��6%   $��Размер��65   6E   �� 5U      $��1��6   $��2��6&   $
 True��63   $��0q�5V   D   ��6   �� $ 
FileLength�$ OneWay=�6D   A� 5X      $��0��6   $��2��6*   $��3��6>    A� Rows$ 1��6K   $	��Top=�6T   $��Center=�6l   $��0,10,0,0q�6�   .+�5Y      �� $��Зашифровать��6   $��5q�6/   $��30,10q�6:   ��6J   �� *EncryptCommand+�� 5Z      $��Расшифровать��6   $��5q�60   $��30,10q�6;   ��6K   �� *DescryptCommand+�� 5[      $��Отменить��6   $��5q�6,   $��30,10q�67   ��6G   �� *CancelCommand+5\      5^      5_      5`      