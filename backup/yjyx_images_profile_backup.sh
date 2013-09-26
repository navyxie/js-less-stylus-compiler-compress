#!/bin/bash
# your mysql login information
# BACKUP_DIR    backup PATH
# BASIC_DIR     basic PATH
# FILE_NAME     file name
# DAYS          backup most days
# -----------------------------
BACKUP_DIR=/home/ubuntu/backup/mysql/images
BASIC_DIR=/var/awangdata/images
FILE_NAME=images
DAYS=30

DATE=`date +%Y-%m-%d-%H`
TAR_FILE="${FILE_NAME}-$DATE.tar.gz"

echo "------------------- Begin backup File ${DATE} -------------------"
echo "1. check the directory for store backup:${BACKUP_DIR} "
if [[ ! -d ${BACKUP_DIR} ]]; then
    echo "    1.1 no directory and create path:${BACKUP_DIR} "
    mkdir -p ${BACKUP_DIR}
fi

echo "2. tar FILES to ${TAR_FILE} "
cd ${BASIC_DIR}
tar -czf "${BACKUP_DIR}/${TAR_FILE}" *

echo "3. delete More than days backup "
cd ${BACKUP_DIR}
find ./ -name "${FILE_NAME}*" -type f -mtime +${DAYS} -exec rm {} \;
echo "------------------- End backup File  -------------------"
exit 0;