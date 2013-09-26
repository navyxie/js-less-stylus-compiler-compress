#!/bin/bash

#for backup polyvore user profile data

DATA_DIR_NAME='polyvoredata'
DATA_DIR_PATH='/var/'$DATA_DIR_NAME
BACKUP_DIR_PATH='/root/backup/polyvore/user_pofile'
BACKUP_FILES_MAX=10

echo '';
echo '';
echo '--------------------------------------';
echo `date`;
echo 'Backup Begin.';
if [ -d $BACKUP_DIR_PATH ]
then
	echo 'Backup dir has already existed. tar && gzip begin.'
	i=0;
	while [ -f "$BACKUP_DIR_PATH/$DATA_DIR_NAME.$i.tar.gz" ]; do
		i=$(($i+1));
	done
	
	if [ $i -ge $(($BACKUP_FILES_MAX*2)) ]
	then
		echo 'data exception.clear data.';
		 j=0;
    while [ $j -lt $BACKUP_FILES_MAX ]; do
	  	rm -f  "$BACKUP_DIR_PATH/$DATA_DIR_NAME.$j.tar.gz" 2>&1;
			j=$(($j+1));
    done
	fi	

	tar -cPf "$BACKUP_DIR_PATH/$DATA_DIR_NAME.$i.tar" "$BACKUP_DIR_PATH/$DATA_DIR_NAME" 2>&1;
	gzip "$BACKUP_DIR_PATH/$DATA_DIR_NAME.$i.tar" 2>&1;

	if [ -f "$BACKUP_DIR_PATH/$DATA_DIR_NAME.$i.tar.gz" ]
	then
		echo 'gzip success.';
		echo "gz filename: $BACKUP_DIR_PATH/$DATA_DIR_NAME.$i.tar.gz";
		if [ $(($i+1)) -eq $BACKUP_FILES_MAX ]
		then
			echo 'clear old data1.';
			j=$(($i+1));
			while [ $j -lt $(($BACKUP_FILES_MAX*2)) ]; do
				rm -f  "$BACKUP_DIR_PATH/$DATA_DIR_NAME.$j.tar.gz" 2>&1;
				j=$(($j+1));	
			done
		elif [  $(($i+1)) -eq $(($BACKUP_FILES_MAX*2)) ]; then
			echo 'clear old data0.';
			j=0;
      while [ $j -lt $BACKUP_FILES_MAX ]; do
	      rm -f  "$BACKUP_DIR_PATH/$DATA_DIR_NAME.$j.tar.gz" 2>&1;
				j=$(($j+1));
      done
		fi
	else
		echo 'gzip failed.'
		exit 1;
	fi
else
	mkdir -p $BACKUP_DIR_PATH
fi

echo "cp $DATA_DIR_PATH to $BACKUP_DIR_PATH";
cp -r "$DATA_DIR_PATH" "$BACKUP_DIR_PATH/$DATA_DIR_NAME-TMP" 2>&1;
if [ -d "$BACKUP_DIR_PATH/$DATA_DIR_NAME_TMP" ]; then
	echo 'clean old data dir';
	rm -rf "$BACKUP_DIR_PATH/$DATA_DIR_NAME" 2>&1;
	mv "$BACKUP_DIR_PATH/$DATA_DIR_NAME-TMP" "$BACKUP_DIR_PATH/$DATA_DIR_NAME" 2>&1;
else
	echo 'cp error.';
fi
echo "done";
exit 0;
