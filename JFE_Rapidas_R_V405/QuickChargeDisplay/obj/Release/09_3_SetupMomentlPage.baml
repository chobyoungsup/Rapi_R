   M S B A M L   `   `   `  ���� M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  K*clr-namespace:QuickChargeDisplay.ConverterQuickChargeDisplay.Converter  7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  )    #QuickChargeDisplay.SetupMomentlPage   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     2local clr-namespace:QuickChargeDisplay     >convert*clr-namespace:QuickChargeDisplay.Converter     /my clr-namespace:QuickChargeDisplay     -     N� Title$  SetupMomentlPage��$	��800��$	��480�� N� 
Background ��  �� ImageSource$8 2image\10_관리자화면\관리자기본배경.png��.���� -    ��Name$ 
Compulsion��.+��� -    ��Name$ logOut��$'��!image/20191018_Add/back_12955.png�� �� Left$	 719��
 �� Top$ 386.344��$��50��$��59.47���� -   $��label1��$ 79��$	 101��$��Time Set��$��22(����	#FFF8EFEF$��40��$	��103���� -   $��label2��$ 78��$	 147��$	��Day��$��22(����	#FFF8EFEF$��40��$��54���� -   $��label3��$ 78��$	 195��$
��Time��$��22(����	#FFF8EFEF$��40��$��64���� -   $��STimeH��$	 158��$	 147��$��24(�$
��Bold"�$��40��$
��2013�� �� TextAlignment$ Center=� �� TextWrapping$
 Wrap=�$��96�����	#FFEFDCDC��Z� �� -   $��BusinessTimeHHRect��$	 158��$	 147��
������ $��39��	 K� ZIndex$	 2��$��96���� -	   $��textBox1��$	 282��$	 148��$��24(�$
��Bold"�$��39��$��12��$ Center=�$
 Wrap=�$��56�����	#FFEFDCDC��Z� �� -
   $��
rectangle1��$	 282��$	 145��
������ $��44��$	 2��$��56���� -   $��textBox2��$	 361��$	 146��$��24(�$
��Bold"�$��40��$��12��$ Center=�$
 Wrap=�$��56�����	#FFEFDCDC��Z� �� -   $��
rectangle2��$	 361��$	 143��
������ $��44��$	 2��$��56���� -   $��textBox3��$	 158��$	 197��$��24(�$
��Bold"�$��40��$��12��$ Center=�$
 Wrap=�$��56�����	#FFEFDCDC��Z� �� -   $��
rectangle3��$	 158��$	 195��
������ $��44��$	 2��$��56���� -   $��textBox4��$	 247��$	 197��$��24(�$
��Bold"�$��40��$��12��$ Center=�$
 Wrap=�$��56�����	#FFEFDCDC��Z� �� -   $��
rectangle4��$	 247��$	 196��
������ $��44��$	 2��$��56���� -   $��textBox5��$	 341��$	 197��$��24(�$
��Bold"�$��40��$��12��$ Center=�$
 Wrap=�$��56�����	#FFEFDCDC��Z� �� -   $��
rectangle5��$	 341��$	 196��
������ $��44��$	 2��$��56���� -   $��label4��$ 78��$	 259��$��Charging Time :��$��22(����	#FFF8EFEF$��40��$	��179���� -   $��TxtCharging��$	 244��$	 259��$��24(�$
��Bold"�$��40��$��30��$ Center=�$
 Wrap=�$��96�����	#FFEFDCDC��Z� �� -   $��RecCharging��$	 244��$	 259��
������ $��40��$	 2��$��96���� -   $��button1��$	 463��$	 145��$��Time Set��$��99��$	��161��$��20(�
���   �$��Yet R)�$��Normal"��� -   $��CahrgeHistory��$	 241��$ 0��$��80��$9��3image\10_관리자화면\아이콘/충전이력.png��$��79���� -   $��TroubleHistory��$	 318��$ 0��$��80��$9��3image\10_관리자화면\아이콘/에러이력.png��$��79���� -   $��PassWord��$	 397��$ 0��$��80��$9��3image\10_관리자화면\아이콘/패스워드.png��$��79���� -   $��LogSampling��$	 720��$ 1��$��80��$9��3image\10_관리자화면\아이콘/로그추출.png��$��79���� -   $��SetupMaxium��$	 562��$ 1��$��78��$?��9image\10_관리자화면\아이콘/최대전류설정.png��$��77���� -   $��TroubleDetails��$ 0��$ 1��$��80��$1��+image/RAPIDAS-R_Monitor/07/관리설정.png��$��78���� -   $��SetupEquipment��$	 478��$ 0��$��80��$7��1image/RAPIDAS-R_Monitor/07/수전전력설정.png��$��82���� -   $��
PowerMeter��$	 161��$ 0��$��80��$9��3image\10_관리자화면\아이콘/언어설정.png��$��83���� -   $��EquipmentInfo��$	 639��$ 1��$��80��$9��3image\10_관리자화면\아이콘/강제충전.png��$��80���� -    $��Langanglab1��$ 0��$ 54��$��管理設定��$��27��$��78�����	#FFF2E9E9
 � ForceCursor
 .  $��Stretch=�$��Center=�$��Bottom=��� -!   $��Langanglab2��$	 720��$ 55��$��	Log抽出��
 .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -"   $��Langanglab3��$ 79��$ 54��$��時間の設定��
 .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -#   $��Langanglab4��$	 161��$ 54��$��言語設定��
 .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -$   $��Langanglab5��$	 242��$ 54��$��充電履歴��
 .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -%   $��Langanglab6��$	 319��$ 53��$��エラー履歴��
 .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -&   $��Langanglab7��$	 398��$ 54��$��パスワード��
 .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -'   $��Langanglab8��$	 478��$ 54��$��受電電力設定��
 .  ���	#FFF2E9E9$��27��$��Center=�$��Bottom=�$��81���� -(   $��Langanglab9��$	 562��$ 54��$��電流設定��
 .  ���	#FFF2E9E9$��27��$��Center=�$��Bottom=�$��78���� -)   $��Langanglab10��$	 640��$ 55��$��Manual��
 .  ���	#FFF2E9E9$��27��$��Center=�$��Bottom=�$��78���� -*   $��Imgmanu��$	 562��$ -1��$��81��$H��B/QuickChargeDisplay;component/image/10_관리자화면/Imgmanu.png��$	��238��$	��Top=� �� StretchDirection$
 Both=��� -+   $��label5��$ 74��$	 356��$��SBC Ver��$��22(����	#FFF8EFEF$��40��$	��179���� -,   $��label6��$	 370��$	 355��$��DSP Ver��$��22(����	#FFF8EFEF$��40��$	��179���� --   $��Tsbcversion��$	 171��$	 356��$��24(�$
��Bold"����	#FFEFDCDC$��40��$��V1.00��$ Center=�$
 Wrap=�$	��124����Z� �� -.   $��Tdspversion��$	 463��$	 355��$��24(�$
��Bold"����	#FFEFDCDC$��40��$��V1.00��$ Center=�$
 Wrap=�$	��124����Z� �� -/   $��label7��$ 71��$	 411��$��Serial Number :��$��22(����	#FFF8EFEF$��40��$	��170���� -0   $��tSerial��$	 247��$	 411��$��24(�$
��Bold"����	#FFEFDCDC$��40��$��
0123456789��$ Center=�$
 Wrap=�$	��312����Z� 