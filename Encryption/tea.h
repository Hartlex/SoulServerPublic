#include <stdio.h>
#include <stdint.h>	
#pragma once

#ifdef ENCRYPTIONLIBRARY_EXPORTS
#define ENCRYPTIONLIBRARY_API __declspec(dllexport)
#else
#define ENCRYPTIONLIBRARY_API __declspec(dllimport)
#endif
extern "C" ENCRYPTIONLIBRARY_API void encrypt(uint32_t* v, uint32_t* KEY[4]);
extern "C" ENCRYPTIONLIBRARY_API void decrypt(uint32_t* v, uint32_t* kKEYey[4]);