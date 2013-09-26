#!/bin/bash
# your mysql login information
# BACKUP_DIR    backup path
# TEMPORARY_DIR temporary path
# DB_NAME   database name
# DB_USER   database user
# DB_PASS   database password
# -----------------------------
BACKUP_DIR=/home/ubuntu/backup/mysql/taobao
TEMPORARY_DIR="${BACKUP_DIR}/recover"
DB_NAME=taobao
DB_USER=root
DB_PASS=1234

echo "------------------- Begin recover -------------------"

echo "1. check and create path :${TEMPORARY_DIR}"
if [[ ! -d $TEMPORARY_DIR ]]; then
    echo "    1.1 no directory and create path "
    mkdir -p ${TEMPORARY_DIR}
fi

echo "2. check your input ${BACKUP_DIR}/$1 "
if [[ $1 == "" ]]
then
    echo "    2.1 please choose recover version "
    echo "    for example : ${DB_NAME}-2012-02-29-11.tar.gz "
    exit 0;
fi

if( ! test -f "${BACKUP_DIR}/$1")
then
    echo "    2.2 Sorry,not find ${1} , please choose recover version "
    echo "    for example : ${DB_NAME}-2012-02-29-11 "
    exit 0;
fi

echo "3. recover $1 to .sql "
cd ${TEMPORARY_DIR}
tar zxvf "${BACKUP_DIR}/$1"

echo "4. recover .sql to database "
mysql -u${DB_USER} -p${DB_PASS} "${DB_NAME}" < ${DB_NAME}.sql

echo "5. del temporary ${DB_NAME}.sql "
rm -f ${DB_NAME}.sql

echo "------------------- End recover -------------------"
exit 0;