#!/bin/bash

#for backup polyvore elgg database

DATABASE_NAME='polyvore'
BACKUP_DIR_PATH='/home/ubuntu/backup'
BACKUP_FILE_NAME="$DATABASE_NAME.sql"
BACKUP_FILES_MAX=10
echo '';
echo '';
echo '--------------------------------------';
echo `date`;
echo 'Backup Begin.';

if [ -d $BACKUP_DIR_PATH ]
then
  echo 'Backup dir has already existed. backup begin.';
  i=0;
  while [ -f "$BACKUP_DIR_PATH/$BACKUP_FILE_NAME.$i.gz" ]; do
    i=$(($i+1));
  done


  if [ $i -ge $(($BACKUP_FILES_MAX*2)) ]
  then
    echo 'data exception.clear data.';
     j=0;
    while [ $j -lt $BACKUP_FILES_MAX ]; do
      rm -f  "$BACKUP_DIR_PATH/$BACKUP_FILE_NAME.$j.gz" 2>&1;
      j=$(($j+1));
    done
  fi

  if [ -f "$BACKUP_DIR_PATH/$BACKUP_FILE_NAME" ]; then
  	cp "$BACKUP_DIR_PATH/$BACKUP_FILE_NAME" "$BACKUP_DIR_PATH/$BACKUP_FILE_NAME.$i" 2>&1;
  	gzip "$BACKUP_DIR_PATH/$BACKUP_FILE_NAME.$i" 2>&1;

  	if [ -f "$BACKUP_DIR_PATH/$BACKUP_FILE_NAME.$i.gz" ]
  	then
    		echo 'gzip success.';
    		echo "gz filename: $BACKUP_DIR_PATH/$BACKUP_FILE_NAME.$i.gz";
    		if [ $(($i+1)) -eq $BACKUP_FILES_MAX ]
    		then
      			echo 'clean old data1.';
      			j=$(($i+1));
      			while [ $j -lt $(($BACKUP_FILES_MAX*2)) ]; do
        			rm -f  "$BACKUP_DIR_PATH/$BACKUP_FILE_NAME.$j.gz" 2>&1;
        			j=$(($j+1));
      			done
    		elif [  $(($i+1)) -eq $(($BACKUP_FILES_MAX*2)) ]; then
      			echo 'clean old data0.';
      			j=0;
     			while [ $j -lt $BACKUP_FILES_MAX ]; do
        			rm -f  "$BACKUP_DIR_PATH/$BACKUP_FILE_NAME.$j.gz" 2>&1;
        			j=$(($j+1));
      			done
    		fi
  	else
    		echo 'gzip failed.';
    		exit 1;
  	fi
  else
  	mkdir -p $BACKUP_DIR_PATH
  fi
fi

echo "mysqldump to $BACKUP_DIR_PATH/$BACKUP_FILE_NAME"
mysqldump -uroot -p1234 --add-drop-table --add-locks $DATABASE_NAME >  "$BACKUP_DIR_PATH/$BACKUP_FILE_NAME-TMP"
if [ -f "$BACKUP_DIR_PATH/$BACKUP_FILE_NAME-TMP" ]; then
  echo 'clean old data';
  rm -rf "$BACKUP_DIR_PATH/$BACKUP_FILE_NAME" 2>&1;
  mv "$BACKUP_DIR_PATH/$BACKUP_FILE_NAME-TMP" "$BACKUP_DIR_PATH/$BACKUP_FILE_NAME" 2>&1;
else
  echo 'mysqldump error.';
fi
echo "done";
exit 0;


