#!/bin/bash

stack_name=media-server-stack
media_server_name=media-server
aws_region_code=us-east-1

vpc_id=$(aws ec2 describe-vpcs --region ${aws_region_code} --filter "Name=isDefault,Values=true" --query "Vpcs[].VpcId" --output text)

if [[ -z $vpc_id ]]
then
    read -p "VPC Id: " vpc_id
fi

aws cloudformation create-stack \
    --stack-name ${stack_name} \
    --region ${aws_region_code} \
    --parameters ParameterKey=MediaServerName,ParameterValue=${media_server_name} \
    --parameters ParameterKey=VpcId,ParameterValue=${vpc_id} \
    --template-body file://server/config/media-server-stack.yaml

aws cloudformation wait stack-create-complete --stack-name ${stack_name} --region ${aws_region_code}

key_id=$(aws ec2 describe-key-pairs --filters Name=key-name,Values=media-server-keypair --query KeyPairs[*].KeyPairId --output text --region ${aws_region_code})

if [ ! -d "server/keys" ]
then
  mkdir server/keys
fi

if test -f server/keys/media-server-keypair-${aws_region_code}.pem; then
  rm server/keys/media-server-keypair-${aws_region_code}.pem
fi

aws ssm get-parameter --name " /ec2/keypair/${key_id}" --with-decryption \
    --query Parameter.Value --region ${aws_region_code} \
    --output text > server/keys/media-server-keypair-${aws_region_code}.pem

echo

aws cloudformation describe-stacks \
    --stack-name ${stack_name} \
    --region ${aws_region_code} \
    --query "Stacks[0].Outputs[?OutputKey=='PublicDnsName'].OutputValue" --output text
