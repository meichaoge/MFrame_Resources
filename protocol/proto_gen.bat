protogen.exe -i:net_packet.proto -o:../client/Assets/Script/Protocol/Packet.cs -ns:protocol
protogen.exe -i:result_codes.proto -o:../client/Assets/Script/Protocol/PacketResultCode.cs -ns:protocol
protogen.exe -i:friend.proto -o:../client/Assets/Script/Protocol/PacketFriend.cs -ns:protocol
protogen.exe -i:shop.proto -o:../client/Assets/Script/Protocol/PacketShop.cs -ns:protocol
protogen.exe -i:common_pb.proto -o:../client/Assets/Script/Protocol/PacketCommonPb.cs -ns:protocol
protogen.exe -i:task.proto -o:../client/Assets/Script/Protocol/PacketTask.cs -ns:protocol
pause