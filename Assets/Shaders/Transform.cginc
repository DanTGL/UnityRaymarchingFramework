float2x2 GetRotationMatrix(float angle) {
    return float2x2(cos(angle), -sin(angle),
                    sin(angle),  cos(angle));
}

inline float3 Transform(float3 pos, float3 rotation) {
    pos.yz = mul(GetRotationMatrix(-rotation.x), pos.yz);
    pos.xz = mul(GetRotationMatrix(rotation.y), pos.xz);
    pos.xy = mul(GetRotationMatrix(-rotation.z), pos.xy);
    return pos;
}