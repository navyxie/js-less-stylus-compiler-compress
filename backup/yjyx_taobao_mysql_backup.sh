#!/bin/bash
# your mysql login information
# DUMP      backup order
# OUT_DIR   output path
# DB_NAME   database name
# DB_USER   database user
# DB_PASS   database password
# DAYS      backup most days
# MINS      backup most minute
# -----------------------------
DUMP=mysqldump
OUT_DIR=/home/ubuntu/backup/mysql/taobao
DB_NAME=taobao
DB_USER=root
DB_PASS=1234
DAYS=30
#MINS=720

DATE=`date +%Y-%m-%d-%H`
OUT_SQL="${DB_NAME}.sql"
TAR_SQL="${DB_NAME}-$DATE.tar.gz"

echo "------------------- Begin backup ${DB_NAME} database ${DATE} -------------------"
echo "1. check the directory for store backup:${OUT_DIR} "
if [[ ! -d $OUT_DIR ]]; then
    echo "    1.1 no directory and create path:${OUT_DIR} "
    mkdir -p $OUT_DIR
fi

#Core of script
cd $OUT_DIR

echo "2. check file ${OUT_SQL} "
if(test -f "${OUT_SQL}")
then
    echo "    2.1 delete file ${OUT_SQL} "
    rm -f $OUT_SQL
fi

echo "3. ${DUMP} database to ${OUT_SQL} "
$DUMP -u$DB_USER -p$DB_PASS --add-drop-table --add-locks $DB_NAME > $OUT_SQL

echo "4. tar ${OUT_SQL} to ${TAR_SQL} "
tar -czf $TAR_SQL ./$OUT_SQL

echo "5. delete More than days backup "
#find ./ -name "${DB_NAME}*" -type f -mmin  +$MINS -exec rm {} \;
find ./ -name "${DB_NAME}*" -type f -mtime +$DAYS -exec rm {} \;
echo "------------------- End backup ${DB_NAME} database -------------------"
exit 0;