   M S B A M L   `   `   `  ����M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  5      K*clr-namespace:QuickChargeDisplay.ConverterQuickChargeDisplay.Converter  5      7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  5      )    #QuickChargeDisplay.SetupMomentlPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      2local clr-namespace:QuickChargeDisplay     5      >convert*clr-namespace:QuickChargeDisplay.Converter     5      /my clr-namespace:QuickChargeDisplay     5      -   5          N� Title$  SetupMomentlPage��5      $	��800��6    $	��480��6,    N� 
Background 5
      �� 5   
    �� ImageSource$8 2image\10_관리자화면\관리자기본배경.png��6   .��5      �� -   5         ��Name$ 
Compulsion��5      .+�5   
   �� -   5         ��Name$ logOut��5      $'��!image/20191018_Add/back_12955.png��6     �� Left$	 719��6K   
 �� Top$ 386.344��6]   $��50��6r   $��59.47��6}   6�   �� 5   
   -   5        $��label1��5   v   $ 79��6   $	 101��6!   $��Time Set��62   $��22(�6E   ���	#FFF8EFEF$��40��6j   $	��103��6�   �� 5   
   -   5        $��label2��5   q   $ 78��6   $	 147��6!   $	��Day��62   $��22(�6@   ���	#FFF8EFEF$��40��6e   $��54��6   �� 5   
   -   5        $��label3��5   r   $ 78��6   $	 195��6!   $
��Time��62   $��22(�6A   ���	#FFF8EFEF$��40��6f   $��64��6�   �� 5   
   -   5        $��STimeH��5   a   $	 158��6   $	 147��6$   $��24(�65   $
��Bold"�6C   $��40��6U   $
��2013��6o    �� TextAlignment$ Center=�6{    �� TextWrapping$
 Wrap=�6�   $��96��6�   ���	#FFEFDCDC��6�   Z� 6�   �� 5   
   -   5        $��BusinessTimeHHRect��5   V   $	 158��6   $	 147��6&   
������ $��39��6J   	 K� ZIndex$	 2��6p   $��96��6�   6�   �� 5   
   -	   5        $��textBox1��5   a   $	 282��6   $	 148��6$   $��24(�65   $
��Bold"�6C   $��39��6U   $��12��6q   $ Center=�6{   $
 Wrap=�6�   $��56��6�   ���	#FFEFDCDC��6�   Z� 6�   �� 5   
   -
   5        $��
rectangle1��5   V   $	 282��6   $	 145��6&   
������ $��44��6J   $	 2��6h   $��56��6y   6�   �� 5   
   -   5        $��textBox2��5   a   $	 361��6   $	 146��6$   $��24(�65   $
��Bold"�6C   $��40��6U   $��12��6q   $ Center=�6{   $
 Wrap=�6�   $��56��6�   ���	#FFEFDCDC��6�   Z� 6�   �� 5   
   -   5        $��
rectangle2��5   V   $	 361��6   $	 143��6&   
������ $��44��6J   $	 2��6h   $��56��6y   6�   �� 5   
   -   5        $��textBox3��5   a   $	 158��6   $	 197��6$   $��24(�65   $
��Bold"�6C   $��40��6U   $��12��6q   $ Center=�6{   $
 Wrap=�6�   $��56��6�   ���	#FFEFDCDC��6�   Z� 6�   �� 5   
   -   5        $��
rectangle3��5   V   $	 158��6   $	 195��6&   
������ $��44��6J   $	 2��6h   $��56��6y   6�   �� 5   
   -   5        $��textBox4��5   a   $	 247��6   $	 197��6$   $��24(�65   $
��Bold"�6C   $��40��6U   $��12��6q   $ Center=�6{   $
 Wrap=�6�   $��56��6�   ���	#FFEFDCDC��6�   Z� 6�   �� 5   
   -   5        $��
rectangle4��5   V   $	 247��6   $	 196��6&   
������ $��44��6J   $	 2��6h   $��56��6y   6�   �� 5   
   -   5        $��textBox5��5   a   $	 341��6   $	 197��6$   $��24(�65   $
��Bold"�6C   $��40��6U   $��12��6q   $ Center=�6{   $
 Wrap=�6�   $��56��6�   ���	#FFEFDCDC��6�   Z� 6�   �� 5   
   -   5        $��
rectangle5��5   V   $	 341��6   $	 196��6&   
������ $��44��6J   $	 2��6h   $��56��6y   6�   �� 5   
   -   5        $��label4��5   }   $ 78��6   $	 259��6!   $��Charging Time :��62   $��22(�6L   ���	#FFF8EFEF$��40��6q   $	��179��6�   �� 5    
   -   5        $��TxtCharging��5    a   $	 244��6   $	 259��6$   $��24(�65   $
��Bold"�6C   $��40��6U   $��30��6t   $ Center=�6~   $
 Wrap=�6�   $��96��6�   ���	#FFEFDCDC��6�   Z� 6�   �� 5!   
   -   5        $��RecCharging��5!   V   $	 244��6   $	 259��6&   
������ $��40��6J   $	 2��6i   $��96��6z   6�   �� 5"   
   -   5        $��button1��5"   S   $	 463��6   $	 145��6#   $��Time Set��64   $��99��6G   $	��161��6b   $��20(�6�   
���   �$��Yet R)�6�   $��Normal"�6�   6�   �� 5&   
   -   5        $��CahrgeHistory��5&   =   $	 241��6   $ 0��6"   $��80��61   $9��3image\10_관리자화면\아이콘/충전이력.png��6R   $��79��6w   6�   �� 5'   
   -   5        $��TroubleHistory��5'   =   $	 318��6   $ 0��6"   $��80��61   $9��3image\10_관리자화면\아이콘/에러이력.png��6S   $��79��6x   6�   �� 5(   
   -   5        $��PassWord��5(   =   $	 397��6   $ 0��6"   $��80��61   $9��3image\10_관리자화면\아이콘/패스워드.png��6M   $��79��6r   6}   �� 5)   
   -   5        $��LogSampling��5)   =   $	 720��6   $ 1��6"   $��80��61   $9��3image\10_관리자화면\아이콘/로그추출.png��6P   $��79��6u   6�   �� 5*   
   -   5        $��SetupMaxium��5*   =   $	 562��6   $ 1��6"   $��78��61   $?��9image\10_관리자화면\아이콘/최대전류설정.png��6P   $��77��6w   6�   �� 5+   
   -   5        $��TroubleDetails��5+   ;   $ 0��6   $ 1��6    $��80��6/   $1��+image/RAPIDAS-R_Monitor/07/관리설정.png��6Q   $��78��6~   6�   �� 5,   
   -   5        $��SetupEquipment��5,   =   $	 478��6   $ 0��6"   $��80��61   $7��1image/RAPIDAS-R_Monitor/07/수전전력설정.png��6S   $��82��6�   6�   �� 5-   
   -   5        $��
PowerMeter��5-   =   $	 161��6   $ 0��6"   $��80��61   $9��3image\10_관리자화면\아이콘/언어설정.png��6O   $��83��6t   6   �� 5.   
   -   5        $��EquipmentInfo��5.   =   $	 639��6   $ 1��6"   $��80��61   $9��3image\10_관리자화면\아이콘/강제충전.png��6R   $��80��6w   6�   �� 5/   
   -    5        $��Langanglab1��5/   K   $ 0��6   $ 54��6    $��管理設定��60   $��27��6?   $��78��6^   ���	#FFF2E9E9
 � ForceCursor
 .  $��Stretch=�6�   $��Center=�6�   $��Bottom=�6�   �� 50   
   -!   5        $��Langanglab2��50   �   $	 720��6   $ 55��6"   $��	Log抽出��62   
 .  ���	#FFF2E9E9$��27��6m   $��Center=�6y   $��78��6�   $��Bottom=�6�   �� 51   
   -"   5        $��Langanglab3��51   �   $ 79��6   $ 54��6!   $��時間の設定��61   
 .  ���	#FFF2E9E9$��27��6l   $��Center=�6x   $��78��6�   $��Bottom=�6�   �� 52   
   -#   5        $��Langanglab4��52   �   $	 161��6   $ 54��6"   $��言語設定��62   
 .  ���	#FFF2E9E9$��27��6l   $��Center=�6x   $��78��6�   $��Bottom=�6�   �� 53   
   -$   5        $��Langanglab5��53   �   $	 242��6   $ 54��6"   $��充電履歴��62   
 .  ���	#FFF2E9E9$��27��6l   $��Center=�6x   $��78��6�   $��Bottom=�6�   �� 54   
   -%   5        $��Langanglab6��54   �   $	 319��6   $ 53��6"   $��エラー履歴��62   
 .  ���	#FFF2E9E9$��27��6m   $��Center=�6y   $��78��6�   $��Bottom=�6�   �� 55   
   -&   5        $��Langanglab7��55   �   $	 398��6   $ 54��6"   $��パスワード��62   
 .  ���	#FFF2E9E9$��27��6m   $��Center=�6y   $��78��6�   $��Bottom=�6�   �� 56   
   -'   5        $��Langanglab8��56   �   $	 478��6   $ 54��6"   $��受電電力設定��62   
 .  ���	#FFF2E9E9$��27��6n   $��Center=�6z   $��Bottom=�6�   $��81��6�   �� 57   
   -(   5        $��Langanglab9��57   �   $	 562��6   $ 54��6"   $��電流設定��62   
 .  ���	#FFF2E9E9$��27��6l   $��Center=�6x   $��Bottom=�6�   $��78��6�   �� 58   
   -)   5        $��Langanglab10��58   �   $	 640��6   $ 55��6"   $��Manual��62   
 .  ���	#FFF2E9E9$��27��6n   $��Center=�6z   $��Bottom=�6�   $��78��6�   �� 59   
   -*   5        $��Imgmanu��59   >   $	 562��6   $ -1��6"   $��81��62   $H��B/QuickChargeDisplay;component/image/10_관리자화면/Imgmanu.png��6M   $	��238��6�   $	��Top=�6�    �� StretchDirection$
 Both=�6�   �� 5:   
   -+   5        $��label5��5:   u   $ 74��6   $	 356��6!   $��SBC Ver��62   $��22(�6D   ���	#FFF8EFEF$��40��6i   $	��179��6�   �� 5;   
   -,   5        $��label6��5;   v   $	 370��6   $	 355��6"   $��DSP Ver��63   $��22(�6E   ���	#FFF8EFEF$��40��6j   $	��179��6�   �� 5<   
   --   5        $��Tsbcversion��5<   �   $	 171��6(   $	 356��6:   $��24(�6K   $
��Bold"�6Y   ���	#FFEFDCDC$��40��6�   $��V1.00��6�   $ Center=�6�   $
 Wrap=�6�   $	��124��6�   ��6   Z� 6�   �� 5=   
   -.   5        $��Tdspversion��5=   �   $	 463��6(   $	 355��6:   $��24(�6K   $
��Bold"�6Y   ���	#FFEFDCDC$��40��6�   $��V1.00��6�   $ Center=�6�   $
 Wrap=�6�   $	��124��6�   ��6   Z� 6�   �� 5>   
   -/   5        $��label7��5>   }   $ 71��6   $	 411��6!   $��Serial Number :��62   $��22(�6L   ���	#FFF8EFEF$��40��6q   $	��170��6�   �� 5?   
   -0   5        $��tSerial��5?   �   $	 247��6(   $	 411��6:   $��24(�6K   $
��Bold"�6Y   ���	#FFEFDCDC$��40��6�   $��
0123456789��6�   $ Center=�6�   $
 Wrap=�6�   $	��312��6�   ��6   Z� 6�   5@      5A      