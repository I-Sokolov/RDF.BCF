using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Linq;
using System.Xml.Linq;

namespace RDF
{
	public enum enum_express_declaration : byte
	{
		__NONE						= 0,
		__ENTITY					= 1,
		__ENUM						= 2,
		__SELECT					= 3,
		__DEFINED_TYPE				= 4,
		__FUNCTION					= 5,
		__PROCEDURE					= 6,
		__GLOBAL_RULE				= 7,
		__WHERE_RULE				= 8
	};

	public enum enum_express_attr_type : byte
	{
		__NONE						= 0,					//	attribute type is unknown here but it may be defined by referenced domain entity
		__BINARY					= 1,
		__BINARY_32					= 2,
		__BOOLEAN					= 3,
		__ENUMERATION				= 4,
		__INTEGER					= 5,
		__LOGICAL					= 6,
		__NUMBER					= 7,
		__REAL						= 8,
		__SELECT					= 9,
		__STRING					= 10,
		__GENERIC					= 11
	};

	public enum enum_express_aggr : byte
	{
		__NONE						= 0,
		__ARRAY						= 1,
		__BAG						= 2,
		__LIST						= 3,
		__SET						= 4,
		__AGGREGATE					= 5						//	generic aggregate
	};

	public enum enum_validation_type : System.UInt64
	{
		__NONE						= 0,
		__KNOWN_ENTITY				= 1 << 0,				//  entity is defined in the schema
		__NO_OF_ARGUMENTS			= 1 << 1,				//	number of arguments
		__ARGUMENT_EXPRESS_TYPE		= 1 << 2,				//	argument value is correct entity, defined type or enumeration value
		__ARGUMENT_PRIM_TYPE		= 1 << 3,				//	argument value has correct primitive type
		__REQUIRED_ARGUMENTS		= 1 << 4,				//	non-optional arguments values are provided
		__ARRGEGATION_EXPECTED		= 1 << 5,				//	aggregation is provided when expected
		__AGGREGATION_NOT_EXPECTED	= 1 << 6,   			//	aggregation is not used when not expected
		__AGGREGATION_SIZE			= 1 << 7,   			//	aggregation size
		__AGGREGATION_UNIQUE		= 1 << 8,				//	elements in aggregations are unique when required
		__COMPLEX_INSTANCE			= 1 << 9,				//	complex instances contains full parent chains
		__REFERENCE_EXISTS			= 1 << 10,				//	referenced instance exists
		__ABSTRACT_ENTITY			= 1 << 11,  			//	abstract entity should not instantiate
		__WHERE_RULE				= 1 << 12,  			//	where-rule check
		__UNIQUE_RULE				= 1 << 13,				//	unique-rule check
		__STAR_USAGE				= 1 << 14,  			//	* is used only for derived arguments
		__CALL_ARGUMENT				= 1 << 15,  			//	validateModel / validateInstance function argument should be model / instance
		__INVALID_TEXT_LITERAL		= 1 << 16,				//	invalid text literal string
		__INTERNAL_ERROR			= ((UInt64)1) << 63   	//	unspecified error
	};

	public enum enum_validation_status : byte
	{
		__NONE						= 0,
		__COMPLETE_ALL				= 1,					//	all issues proceed
		__COMPLETE_NOT_ALL			= 2,					//	completed but some issues were excluded by option settings
		__TIME_EXCEED				= 3,					//	validation was finished because of reach time limit
		__COUNT_EXCEED				= 4						//	validation was finished because of reach of issue's numbers limit
	};

	class ifcengine
	{
		public const int sdaiTYPE			 = 0;			//	C++ API generator specific

		public const Int64 flagbit0			 = 1;			//	2^^0    0000.0000..0000.0001
		public const Int64 flagbit1			 = 2;			//	2^^1    0000.0000..0000.0010
		public const Int64 flagbit2			 = 4;			//	2^^2    0000.0000..0000.0100
		public const Int64 flagbit3			 = 8;			//	2^^3    0000.0000..0000.1000
		public const Int64 flagbit4			 = 16;			//	2^^4    0000.0000..0001.0000
		public const Int64 flagbit5			 = 32;			//	2^^5    0000.0000..0010.0000
		public const Int64 flagbit6			 = 64;			//	2^^6    0000.0000..0100.0000
		public const Int64 flagbit7			 = 128;			//	2^^7    0000.0000..1000.0000
		public const Int64 flagbit8			 = 256;			//	2^^8    0000.0001..0000.0000
		public const Int64 flagbit9			 = 512;			//	2^^9    0000.0010..0000.0000
		public const Int64 flagbit10		 = 1024;		//	2^^10   0000.0100..0000.0000
		public const Int64 flagbit11		 = 2048;		//	2^^11   0000.1000..0000.0000
		public const Int64 flagbit12		 = 4096;		//	2^^12   0001.0000..0000.0000
		public const Int64 flagbit13		 = 8192;		//	2^^13   0010.0000..0000.0000
		public const Int64 flagbit14		 = 16384;		//	2^^14   0100.0000..0000.0000
		public const Int64 flagbit15		 = 32768;		//	2^^15   1000.0000..0000.0000

		public const Int64 sdaiADB			 = 1;
		public const Int64 sdaiAGGR			 = sdaiADB + 1;
		public const Int64 sdaiBINARY		 = sdaiAGGR + 1;
		public const Int64 sdaiBOOLEAN		 = sdaiBINARY + 1;
		public const Int64 sdaiENUM			 = sdaiBOOLEAN + 1;
		public const Int64 sdaiINSTANCE		 = sdaiENUM + 1;
		public const Int64 sdaiINTEGER		 = sdaiINSTANCE + 1;
		public const Int64 sdaiLOGICAL		 = sdaiINTEGER + 1;
		public const Int64 sdaiREAL			 = sdaiLOGICAL + 1;
		public const Int64 sdaiSTRING		 = sdaiREAL + 1;
		public const Int64 sdaiUNICODE		 = sdaiSTRING + 1;
		public const Int64 sdaiEXPRESSSTRING = sdaiUNICODE + 1;
		public const Int64 engiGLOBALID		 = sdaiEXPRESSSTRING + 1;

#if ANDROID
		public const string IFCEngineDLL = @"libifcengine.so";
#elif IOS
		public const string IFCEngineDLL = @"__Internal";
#elif WINDOWS || NET6_0_WINDOWS || NETCOREAPP || NET
		public const string IFCEngineDLL = @"Resources\lib\Windows\ifcengine.dll";
#else
		public const string IFCEngineDLL = 
			DeviceInfo.Platform == DevicePlatform.Android ? @"libifcengine.so" :
			DeviceInfo.Platform == DevicePlatform.iOS ? @"__Internal" :
			@"Resources\lib\Windows\ifcengine.dll";
#endif


        //
        //  Instance Header API Calls
        //

		/// <summary>
		///		SetSPFFHeader                                           (https://rdf.bg/ifcdoc/CS64/SetSPFFHeader.html)
		///
		///	This call is an aggregate of several SetSPFFHeaderItem calls. In several cases the header can be set easily with this call. In case an argument is zero, this argument will not be updated, i.e. it will not be filled with 0.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "SetSPFFHeader")]
		public static extern void x86_SetSPFFHeader(Int32 model, string description, string implementationLevel, string name, string timeStamp, string author, string organization, string preprocessorVersion, string originatingSystem, string authorization, string fileSchema);

		[DllImport(IFCEngineDLL, EntryPoint = "SetSPFFHeader")]
		public static extern void x64_SetSPFFHeader(Int64 model, string description, string implementationLevel, string name, string timeStamp, string author, string organization, string preprocessorVersion, string originatingSystem, string authorization, string fileSchema);

		public static void SetSPFFHeader(Int64 model, string description, string implementationLevel, string name, string timeStamp, string author, string organization, string preprocessorVersion, string originatingSystem, string authorization, string fileSchema)
		{
			if (IntPtr.Size == 4)
			{
				x86_SetSPFFHeader((Int32)model, description, implementationLevel, name, timeStamp, author, organization, preprocessorVersion, originatingSystem, authorization, fileSchema);
			}
			else
			{
				x64_SetSPFFHeader(model, description, implementationLevel, name, timeStamp, author, organization, preprocessorVersion, originatingSystem, authorization, fileSchema);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "SetSPFFHeader")]
		public static extern void x86_SetSPFFHeader(Int32 model, byte[] description, byte[] implementationLevel, byte[] name, byte[] timeStamp, byte[] author, byte[] organization, byte[] preprocessorVersion, byte[] originatingSystem, byte[] authorization, byte[] fileSchema);

		[DllImport(IFCEngineDLL, EntryPoint = "SetSPFFHeader")]
		public static extern void x64_SetSPFFHeader(Int64 model, byte[] description, byte[] implementationLevel, byte[] name, byte[] timeStamp, byte[] author, byte[] organization, byte[] preprocessorVersion, byte[] originatingSystem, byte[] authorization, byte[] fileSchema);

		public static void SetSPFFHeader(Int64 model, byte[] description, byte[] implementationLevel, byte[] name, byte[] timeStamp, byte[] author, byte[] organization, byte[] preprocessorVersion, byte[] originatingSystem, byte[] authorization, byte[] fileSchema)
		{
			if (IntPtr.Size == 4)
			{
				x86_SetSPFFHeader((Int32)model, description, implementationLevel, name, timeStamp, author, organization, preprocessorVersion, originatingSystem, authorization, fileSchema);
			}
			else
			{
				x64_SetSPFFHeader(model, description, implementationLevel, name, timeStamp, author, organization, preprocessorVersion, originatingSystem, authorization, fileSchema);
			}
		}

		/// <summary>
		///		SetSPFFHeaderItem                                       (https://rdf.bg/ifcdoc/CS64/SetSPFFHeaderItem.html)
		///
		///	This call can be used to write a specific header item, the source code example is larger to show and explain how this call can be used.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "SetSPFFHeaderItem")]
		public static extern Int32 x86_SetSPFFHeaderItem(Int32 model, Int32 itemIndex, Int32 itemSubIndex, Int32 valueType, out IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "SetSPFFHeaderItem")]
		public static extern Int64 x64_SetSPFFHeaderItem(Int64 model, Int64 itemIndex, Int64 itemSubIndex, Int64 valueType, out IntPtr value);

		public static Int64 SetSPFFHeaderItem(Int64 model, Int64 itemIndex, Int64 itemSubIndex, Int64 valueType, out IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_SetSPFFHeaderItem((Int32)model, (Int32)itemIndex, (Int32)itemSubIndex, (Int32)valueType, out IntPtr _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_SetSPFFHeaderItem(model, itemIndex, itemSubIndex, valueType, out value);
			}
		}

		/// <summary>
		///		GetSPFFHeaderItem                                       (https://rdf.bg/ifcdoc/CS64/GetSPFFHeaderItem.html)
		///
		///	This call can be used to read a specific header item, the source code example is larger to show and explain how this call can be used.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "GetSPFFHeaderItem")]
		public static extern Int32 x86_GetSPFFHeaderItem(Int32 model, Int32 itemIndex, Int32 itemSubIndex, Int32 valueType, out IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "GetSPFFHeaderItem")]
		public static extern Int64 x64_GetSPFFHeaderItem(Int64 model, Int64 itemIndex, Int64 itemSubIndex, Int64 valueType, out IntPtr value);

		public static Int64 GetSPFFHeaderItem(Int64 model, Int64 itemIndex, Int64 itemSubIndex, Int64 valueType, out IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_GetSPFFHeaderItem((Int32)model, (Int32)itemIndex, (Int32)itemSubIndex, (Int32)valueType, out IntPtr _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_GetSPFFHeaderItem(model, itemIndex, itemSubIndex, valueType, out value);
			}
		}

		/// <summary>
		///		GetDateTime                                             (https://rdf.bg/ifcdoc/CS64/GetDateTime.html)
		///
		///	Returns an current date and time according to ISO 8601 without time zone, i.e. formatted as '2099-12-31T23:59:59'.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "GetDateTime")]
		public static extern IntPtr x86_GetDateTime(Int32 model, out IntPtr dateTimeStamp);

		[DllImport(IFCEngineDLL, EntryPoint = "GetDateTime")]
		public static extern IntPtr x64_GetDateTime(Int64 model, out IntPtr dateTimeStamp);

		public static IntPtr GetDateTime(Int64 model, out IntPtr dateTimeStamp)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_GetDateTime((Int32)model, out IntPtr _dateTimeStamp);
				dateTimeStamp = _dateTimeStamp;
				return _result;
			}
			else
			{
				return x64_GetDateTime(model, out dateTimeStamp);
			}
		}

		/// <summary>
		///		GetLibraryIdentifier                                    (https://rdf.bg/ifcdoc/CS64/GetLibraryIdentifier.html)
		///
		///	Returns an identifier for the current instance of this library including date stamp and revision number.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "GetLibraryIdentifier")]
		public static extern IntPtr x86_GetLibraryIdentifier(out IntPtr libraryIdentifier);

		[DllImport(IFCEngineDLL, EntryPoint = "GetLibraryIdentifier")]
		public static extern IntPtr x64_GetLibraryIdentifier(out IntPtr libraryIdentifier);

		public static IntPtr GetLibraryIdentifier(out IntPtr libraryIdentifier)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_GetLibraryIdentifier(out IntPtr _libraryIdentifier);
				libraryIdentifier = _libraryIdentifier;
				return _result;
			}
			else
			{
				return x64_GetLibraryIdentifier(out libraryIdentifier);
			}
		}

		/// <summary>
		///		GetSchemaName                                           (https://rdf.bg/ifcdoc/CS64/GetSchemaName.html)
		///
		///	Returns the value as defined by SCHEMA in the loaded EXPRESS schema.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "GetSchemaName")]
		public static extern IntPtr x86_GetSchemaName(Int32 model, out IntPtr schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "GetSchemaName")]
		public static extern IntPtr x64_GetSchemaName(Int64 model, out IntPtr schemaName);

		public static IntPtr GetSchemaName(Int64 model, out IntPtr schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_GetSchemaName((Int32)model, out IntPtr _schemaName);
				schemaName = _schemaName;
				return _result;
			}
			else
			{
				return x64_GetSchemaName(model, out schemaName);
			}
		}

		/// <summary>
		///		engiSetMappingSupport                                   (https://rdf.bg/ifcdoc/CS64/engiSetMappingSupport.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiSetMappingSupport")]
		public static extern byte x86_engiSetMappingSupport(Int32 entity, bool enable);

		[DllImport(IFCEngineDLL, EntryPoint = "engiSetMappingSupport")]
		public static extern byte x64_engiSetMappingSupport(Int64 entity, bool enable);

		public static byte engiSetMappingSupport(Int64 entity, bool enable)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiSetMappingSupport((Int32)entity, enable);
				return _result;
			}
			else
			{
				return x64_engiSetMappingSupport(entity, enable);
			}
		}

		/// <summary>
		///		engiGetMappingSupport                                   (https://rdf.bg/ifcdoc/CS64/engiGetMappingSupport.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetMappingSupport")]
		public static extern byte x86_engiGetMappingSupport(Int32 entity);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetMappingSupport")]
		public static extern byte x64_engiGetMappingSupport(Int64 entity);

		public static byte engiGetMappingSupport(Int64 entity)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetMappingSupport((Int32)entity);
				return _result;
			}
			else
			{
				return x64_engiGetMappingSupport(entity);
			}
		}

        //
        //  File IO API Calls
        //

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate Int64 ReadCallBackFunction(IntPtr value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void WriteCallBackFunction(IntPtr value, Int64 size);

		/// <summary>
		///		sdaiCreateModelBN                                       (https://rdf.bg/ifcdoc/CS64/sdaiCreateModelBN.html)
		///
		///	This function creates and empty model (we expect with a schema file given).
		///	Attributes repository and fileName will be ignored, they are their because of backward compatibility.
		///	A handle to the model will be returned, or 0 in case something went wrong.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateModelBN")]
		public static extern Int32 x86_sdaiCreateModelBN(Int32 repository, string fileName, string schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateModelBN")]
		public static extern Int64 x64_sdaiCreateModelBN(Int64 repository, string fileName, string schemaName);

		public static Int64 sdaiCreateModelBN(Int64 repository, string fileName, string schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateModelBN((Int32)repository, fileName, schemaName);
				return _result;
			}
			else
			{
				return x64_sdaiCreateModelBN(repository, fileName, schemaName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateModelBN")]
		public static extern Int32 x86_sdaiCreateModelBN(Int32 repository, string fileName, byte[] schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateModelBN")]
		public static extern Int64 x64_sdaiCreateModelBN(Int64 repository, string fileName, byte[] schemaName);

		public static Int64 sdaiCreateModelBN(Int64 repository, string fileName, byte[] schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateModelBN((Int32)repository, fileName, schemaName);
				return _result;
			}
			else
			{
				return x64_sdaiCreateModelBN(repository, fileName, schemaName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateModelBN")]
		public static extern Int32 x86_sdaiCreateModelBN(Int32 repository, byte[] fileName, string schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateModelBN")]
		public static extern Int64 x64_sdaiCreateModelBN(Int64 repository, byte[] fileName, string schemaName);

		public static Int64 sdaiCreateModelBN(Int64 repository, byte[] fileName, string schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateModelBN((Int32)repository, fileName, schemaName);
				return _result;
			}
			else
			{
				return x64_sdaiCreateModelBN(repository, fileName, schemaName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateModelBN")]
		public static extern Int32 x86_sdaiCreateModelBN(Int32 repository, byte[] fileName, byte[] schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateModelBN")]
		public static extern Int64 x64_sdaiCreateModelBN(Int64 repository, byte[] fileName, byte[] schemaName);

		public static Int64 sdaiCreateModelBN(Int64 repository, byte[] fileName, byte[] schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateModelBN((Int32)repository, fileName, schemaName);
				return _result;
			}
			else
			{
				return x64_sdaiCreateModelBN(repository, fileName, schemaName);
			}
		}

		/// <summary>
		///		sdaiCreateModelBNUnicode                                (https://rdf.bg/ifcdoc/CS64/sdaiCreateModelBNUnicode.html)
		///
		///	This function creates and empty model (we expect with a schema file given).
		///	Attributes repository and fileName will be ignored, they are their because of backward compatibility.
		///	A handle to the model will be returned, or 0 in case something went wrong.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateModelBNUnicode")]
		public static extern Int32 x86_sdaiCreateModelBNUnicode(Int32 repository, string fileName, string schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateModelBNUnicode")]
		public static extern Int64 x64_sdaiCreateModelBNUnicode(Int64 repository, string fileName, string schemaName);

		public static Int64 sdaiCreateModelBNUnicode(Int64 repository, string fileName, string schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateModelBNUnicode((Int32)repository, fileName, schemaName);
				return _result;
			}
			else
			{
				return x64_sdaiCreateModelBNUnicode(repository, fileName, schemaName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateModelBNUnicode")]
		public static extern Int32 x86_sdaiCreateModelBNUnicode(Int32 repository, string fileName, byte[] schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateModelBNUnicode")]
		public static extern Int64 x64_sdaiCreateModelBNUnicode(Int64 repository, string fileName, byte[] schemaName);

		public static Int64 sdaiCreateModelBNUnicode(Int64 repository, string fileName, byte[] schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateModelBNUnicode((Int32)repository, fileName, schemaName);
				return _result;
			}
			else
			{
				return x64_sdaiCreateModelBNUnicode(repository, fileName, schemaName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateModelBNUnicode")]
		public static extern Int32 x86_sdaiCreateModelBNUnicode(Int32 repository, byte[] fileName, string schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateModelBNUnicode")]
		public static extern Int64 x64_sdaiCreateModelBNUnicode(Int64 repository, byte[] fileName, string schemaName);

		public static Int64 sdaiCreateModelBNUnicode(Int64 repository, byte[] fileName, string schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateModelBNUnicode((Int32)repository, fileName, schemaName);
				return _result;
			}
			else
			{
				return x64_sdaiCreateModelBNUnicode(repository, fileName, schemaName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateModelBNUnicode")]
		public static extern Int32 x86_sdaiCreateModelBNUnicode(Int32 repository, byte[] fileName, byte[] schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateModelBNUnicode")]
		public static extern Int64 x64_sdaiCreateModelBNUnicode(Int64 repository, byte[] fileName, byte[] schemaName);

		public static Int64 sdaiCreateModelBNUnicode(Int64 repository, byte[] fileName, byte[] schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateModelBNUnicode((Int32)repository, fileName, schemaName);
				return _result;
			}
			else
			{
				return x64_sdaiCreateModelBNUnicode(repository, fileName, schemaName);
			}
		}

		/// <summary>
		///		sdaiOpenModelBN                                         (https://rdf.bg/ifcdoc/CS64/sdaiOpenModelBN.html)
		///
		///	This function opens the model on location fileName.
		///	Attribute repository will be ignored, they are their because of backward compatibility.
		///	A handle to the model will be returned, or 0 in case something went wrong.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiOpenModelBN")]
		public static extern Int32 x86_sdaiOpenModelBN(Int32 repository, string fileName, string schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiOpenModelBN")]
		public static extern Int64 x64_sdaiOpenModelBN(Int64 repository, string fileName, string schemaName);

		public static Int64 sdaiOpenModelBN(Int64 repository, string fileName, string schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiOpenModelBN((Int32)repository, fileName, schemaName);
				return _result;
			}
			else
			{
				return x64_sdaiOpenModelBN(repository, fileName, schemaName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiOpenModelBN")]
		public static extern Int32 x86_sdaiOpenModelBN(Int32 repository, string fileName, byte[] schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiOpenModelBN")]
		public static extern Int64 x64_sdaiOpenModelBN(Int64 repository, string fileName, byte[] schemaName);

		public static Int64 sdaiOpenModelBN(Int64 repository, string fileName, byte[] schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiOpenModelBN((Int32)repository, fileName, schemaName);
				return _result;
			}
			else
			{
				return x64_sdaiOpenModelBN(repository, fileName, schemaName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiOpenModelBN")]
		public static extern Int32 x86_sdaiOpenModelBN(Int32 repository, byte[] fileName, string schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiOpenModelBN")]
		public static extern Int64 x64_sdaiOpenModelBN(Int64 repository, byte[] fileName, string schemaName);

		public static Int64 sdaiOpenModelBN(Int64 repository, byte[] fileName, string schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiOpenModelBN((Int32)repository, fileName, schemaName);
				return _result;
			}
			else
			{
				return x64_sdaiOpenModelBN(repository, fileName, schemaName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiOpenModelBN")]
		public static extern Int32 x86_sdaiOpenModelBN(Int32 repository, byte[] fileName, byte[] schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiOpenModelBN")]
		public static extern Int64 x64_sdaiOpenModelBN(Int64 repository, byte[] fileName, byte[] schemaName);

		public static Int64 sdaiOpenModelBN(Int64 repository, byte[] fileName, byte[] schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiOpenModelBN((Int32)repository, fileName, schemaName);
				return _result;
			}
			else
			{
				return x64_sdaiOpenModelBN(repository, fileName, schemaName);
			}
		}

		/// <summary>
		///		sdaiOpenModelBNUnicode                                  (https://rdf.bg/ifcdoc/CS64/sdaiOpenModelBNUnicode.html)
		///
		///	This function opens the model on location fileName.
		///	Attribute repository will be ignored, they are their because of backward compatibility.
		///	A handle to the model will be returned, or 0 in case something went wrong.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiOpenModelBNUnicode")]
		public static extern Int32 x86_sdaiOpenModelBNUnicode(Int32 repository, string fileName, string schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiOpenModelBNUnicode")]
		public static extern Int64 x64_sdaiOpenModelBNUnicode(Int64 repository, string fileName, string schemaName);

		public static Int64 sdaiOpenModelBNUnicode(Int64 repository, string fileName, string schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiOpenModelBNUnicode((Int32)repository, fileName, schemaName);
				return _result;
			}
			else
			{
				return x64_sdaiOpenModelBNUnicode(repository, fileName, schemaName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiOpenModelBNUnicode")]
		public static extern Int32 x86_sdaiOpenModelBNUnicode(Int32 repository, string fileName, byte[] schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiOpenModelBNUnicode")]
		public static extern Int64 x64_sdaiOpenModelBNUnicode(Int64 repository, string fileName, byte[] schemaName);

		public static Int64 sdaiOpenModelBNUnicode(Int64 repository, string fileName, byte[] schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiOpenModelBNUnicode((Int32)repository, fileName, schemaName);
				return _result;
			}
			else
			{
				return x64_sdaiOpenModelBNUnicode(repository, fileName, schemaName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiOpenModelBNUnicode")]
		public static extern Int32 x86_sdaiOpenModelBNUnicode(Int32 repository, byte[] fileName, string schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiOpenModelBNUnicode")]
		public static extern Int64 x64_sdaiOpenModelBNUnicode(Int64 repository, byte[] fileName, string schemaName);

		public static Int64 sdaiOpenModelBNUnicode(Int64 repository, byte[] fileName, string schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiOpenModelBNUnicode((Int32)repository, fileName, schemaName);
				return _result;
			}
			else
			{
				return x64_sdaiOpenModelBNUnicode(repository, fileName, schemaName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiOpenModelBNUnicode")]
		public static extern Int32 x86_sdaiOpenModelBNUnicode(Int32 repository, byte[] fileName, byte[] schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiOpenModelBNUnicode")]
		public static extern Int64 x64_sdaiOpenModelBNUnicode(Int64 repository, byte[] fileName, byte[] schemaName);

		public static Int64 sdaiOpenModelBNUnicode(Int64 repository, byte[] fileName, byte[] schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiOpenModelBNUnicode((Int32)repository, fileName, schemaName);
				return _result;
			}
			else
			{
				return x64_sdaiOpenModelBNUnicode(repository, fileName, schemaName);
			}
		}

		/// <summary>
		///		engiOpenModelByStream                                   (https://rdf.bg/ifcdoc/CS64/engiOpenModelByStream.html)
		///
		///	This function opens the model via a stream.
		///	Attribute repository will be ignored, they are their because of backward compatibility.
		///	A handle to the model will be returned, or 0 in case something went wrong.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiOpenModelByStream")]
		public static extern Int32 x86_engiOpenModelByStream(Int32 repository, [MarshalAs(UnmanagedType.FunctionPtr)] WriteCallBackFunction callback, string schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiOpenModelByStream")]
		public static extern Int64 x64_engiOpenModelByStream(Int64 repository, [MarshalAs(UnmanagedType.FunctionPtr)] WriteCallBackFunction callback, string schemaName);

		public static Int64 engiOpenModelByStream(Int64 repository, [MarshalAs(UnmanagedType.FunctionPtr)] WriteCallBackFunction callback, string schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiOpenModelByStream((Int32)repository, callback, schemaName);
				return _result;
			}
			else
			{
				return x64_engiOpenModelByStream(repository, callback, schemaName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiOpenModelByStream")]
		public static extern Int32 x86_engiOpenModelByStream(Int32 repository, [MarshalAs(UnmanagedType.FunctionPtr)] WriteCallBackFunction callback, byte[] schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiOpenModelByStream")]
		public static extern Int64 x64_engiOpenModelByStream(Int64 repository, [MarshalAs(UnmanagedType.FunctionPtr)] WriteCallBackFunction callback, byte[] schemaName);

		public static Int64 engiOpenModelByStream(Int64 repository, [MarshalAs(UnmanagedType.FunctionPtr)] WriteCallBackFunction callback, byte[] schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiOpenModelByStream((Int32)repository, callback, schemaName);
				return _result;
			}
			else
			{
				return x64_engiOpenModelByStream(repository, callback, schemaName);
			}
		}

		/// <summary>
		///		engiOpenModelByArray                                    (https://rdf.bg/ifcdoc/CS64/engiOpenModelByArray.html)
		///
		///	This function opens the model via an array.
		///	Attribute repository will be ignored, they are their because of backward compatibility.
		///	A handle to the model will be returned, or 0 in case something went wrong.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiOpenModelByArray")]
		public static extern Int32 x86_engiOpenModelByArray(Int32 repository, byte[] content, Int32 size, string schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiOpenModelByArray")]
		public static extern Int64 x64_engiOpenModelByArray(Int64 repository, byte[] content, Int64 size, string schemaName);

		public static Int64 engiOpenModelByArray(Int64 repository, byte[] content, Int64 size, string schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiOpenModelByArray((Int32)repository, content, (Int32)size, schemaName);
				return _result;
			}
			else
			{
				return x64_engiOpenModelByArray(repository, content, size, schemaName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiOpenModelByArray")]
		public static extern Int32 x86_engiOpenModelByArray(Int32 repository, byte[] content, Int32 size, byte[] schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiOpenModelByArray")]
		public static extern Int64 x64_engiOpenModelByArray(Int64 repository, byte[] content, Int64 size, byte[] schemaName);

		public static Int64 engiOpenModelByArray(Int64 repository, byte[] content, Int64 size, byte[] schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiOpenModelByArray((Int32)repository, content, (Int32)size, schemaName);
				return _result;
			}
			else
			{
				return x64_engiOpenModelByArray(repository, content, size, schemaName);
			}
		}

		/// <summary>
		///		sdaiSaveModelBN                                         (https://rdf.bg/ifcdoc/CS64/sdaiSaveModelBN.html)
		///
		///	This function saves the model (char file name).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelBN")]
		public static extern void x86_sdaiSaveModelBN(Int32 model, string fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelBN")]
		public static extern void x64_sdaiSaveModelBN(Int64 model, string fileName);

		public static void sdaiSaveModelBN(Int64 model, string fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiSaveModelBN((Int32)model, fileName);
			}
			else
			{
				x64_sdaiSaveModelBN(model, fileName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelBN")]
		public static extern void x86_sdaiSaveModelBN(Int32 model, byte[] fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelBN")]
		public static extern void x64_sdaiSaveModelBN(Int64 model, byte[] fileName);

		public static void sdaiSaveModelBN(Int64 model, byte[] fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiSaveModelBN((Int32)model, fileName);
			}
			else
			{
				x64_sdaiSaveModelBN(model, fileName);
			}
		}

		/// <summary>
		///		sdaiSaveModelBNUnicode                                  (https://rdf.bg/ifcdoc/CS64/sdaiSaveModelBNUnicode.html)
		///
		///	This function saves the model (wchar, i.e. Unicode file name).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelBNUnicode")]
		public static extern void x86_sdaiSaveModelBNUnicode(Int32 model, string fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelBNUnicode")]
		public static extern void x64_sdaiSaveModelBNUnicode(Int64 model, string fileName);

		public static void sdaiSaveModelBNUnicode(Int64 model, string fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiSaveModelBNUnicode((Int32)model, fileName);
			}
			else
			{
				x64_sdaiSaveModelBNUnicode(model, fileName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelBNUnicode")]
		public static extern void x86_sdaiSaveModelBNUnicode(Int32 model, byte[] fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelBNUnicode")]
		public static extern void x64_sdaiSaveModelBNUnicode(Int64 model, byte[] fileName);

		public static void sdaiSaveModelBNUnicode(Int64 model, byte[] fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiSaveModelBNUnicode((Int32)model, fileName);
			}
			else
			{
				x64_sdaiSaveModelBNUnicode(model, fileName);
			}
		}

		/// <summary>
		///		engiSaveModelByStream                                   (https://rdf.bg/ifcdoc/CS64/engiSaveModelByStream.html)
		///
		///	This function saves the model as a stream.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiSaveModelByStream")]
		public static extern void x86_engiSaveModelByStream(Int32 model, [MarshalAs(UnmanagedType.FunctionPtr)] WriteCallBackFunction callback, Int32 size);

		[DllImport(IFCEngineDLL, EntryPoint = "engiSaveModelByStream")]
		public static extern void x64_engiSaveModelByStream(Int64 model, [MarshalAs(UnmanagedType.FunctionPtr)] WriteCallBackFunction callback, Int64 size);

		public static void engiSaveModelByStream(Int64 model, [MarshalAs(UnmanagedType.FunctionPtr)] WriteCallBackFunction callback, Int64 size)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiSaveModelByStream((Int32)model, callback, (Int32)size);
			}
			else
			{
				x64_engiSaveModelByStream(model, callback, size);
			}
		}

		/// <summary>
		///		engiSaveModelByArray                                    (https://rdf.bg/ifcdoc/CS64/engiSaveModelByArray.html)
		///
		///	This function saves the model as an array.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiSaveModelByArray")]
		public static extern void x86_engiSaveModelByArray(Int32 model, byte[] content, out Int32 size);

		[DllImport(IFCEngineDLL, EntryPoint = "engiSaveModelByArray")]
		public static extern void x64_engiSaveModelByArray(Int64 model, byte[] content, out Int64 size);

		public static void engiSaveModelByArray(Int64 model, byte[] content, out Int64 size)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiSaveModelByArray((Int32)model, content, out Int32 _size);
				size = _size;
			}
			else
			{
				x64_engiSaveModelByArray(model, content, out size);
			}
		}

		/// <summary>
		///		sdaiSaveModelAsXmlBN                                    (https://rdf.bg/ifcdoc/CS64/sdaiSaveModelAsXmlBN.html)
		///
		///	This function saves the model as XML according to IFC2x3's way of XML serialization (char file name).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsXmlBN")]
		public static extern void x86_sdaiSaveModelAsXmlBN(Int32 model, string fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsXmlBN")]
		public static extern void x64_sdaiSaveModelAsXmlBN(Int64 model, string fileName);

		public static void sdaiSaveModelAsXmlBN(Int64 model, string fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiSaveModelAsXmlBN((Int32)model, fileName);
			}
			else
			{
				x64_sdaiSaveModelAsXmlBN(model, fileName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsXmlBN")]
		public static extern void x86_sdaiSaveModelAsXmlBN(Int32 model, byte[] fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsXmlBN")]
		public static extern void x64_sdaiSaveModelAsXmlBN(Int64 model, byte[] fileName);

		public static void sdaiSaveModelAsXmlBN(Int64 model, byte[] fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiSaveModelAsXmlBN((Int32)model, fileName);
			}
			else
			{
				x64_sdaiSaveModelAsXmlBN(model, fileName);
			}
		}

		/// <summary>
		///		sdaiSaveModelAsXmlBNUnicode                             (https://rdf.bg/ifcdoc/CS64/sdaiSaveModelAsXmlBNUnicode.html)
		///
		///	This function saves the model as XML according to IFC2x3's way of XML serialization (wchar, i.e. Unicode file name).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsXmlBNUnicode")]
		public static extern void x86_sdaiSaveModelAsXmlBNUnicode(Int32 model, string fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsXmlBNUnicode")]
		public static extern void x64_sdaiSaveModelAsXmlBNUnicode(Int64 model, string fileName);

		public static void sdaiSaveModelAsXmlBNUnicode(Int64 model, string fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiSaveModelAsXmlBNUnicode((Int32)model, fileName);
			}
			else
			{
				x64_sdaiSaveModelAsXmlBNUnicode(model, fileName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsXmlBNUnicode")]
		public static extern void x86_sdaiSaveModelAsXmlBNUnicode(Int32 model, byte[] fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsXmlBNUnicode")]
		public static extern void x64_sdaiSaveModelAsXmlBNUnicode(Int64 model, byte[] fileName);

		public static void sdaiSaveModelAsXmlBNUnicode(Int64 model, byte[] fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiSaveModelAsXmlBNUnicode((Int32)model, fileName);
			}
			else
			{
				x64_sdaiSaveModelAsXmlBNUnicode(model, fileName);
			}
		}

		/// <summary>
		///		sdaiSaveModelAsSimpleXmlBN                              (https://rdf.bg/ifcdoc/CS64/sdaiSaveModelAsSimpleXmlBN.html)
		///
		///	This function saves the model as XML according to IFC4's way of XML serialization (char file name).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsSimpleXmlBN")]
		public static extern void x86_sdaiSaveModelAsSimpleXmlBN(Int32 model, string fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsSimpleXmlBN")]
		public static extern void x64_sdaiSaveModelAsSimpleXmlBN(Int64 model, string fileName);

		public static void sdaiSaveModelAsSimpleXmlBN(Int64 model, string fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiSaveModelAsSimpleXmlBN((Int32)model, fileName);
			}
			else
			{
				x64_sdaiSaveModelAsSimpleXmlBN(model, fileName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsSimpleXmlBN")]
		public static extern void x86_sdaiSaveModelAsSimpleXmlBN(Int32 model, byte[] fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsSimpleXmlBN")]
		public static extern void x64_sdaiSaveModelAsSimpleXmlBN(Int64 model, byte[] fileName);

		public static void sdaiSaveModelAsSimpleXmlBN(Int64 model, byte[] fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiSaveModelAsSimpleXmlBN((Int32)model, fileName);
			}
			else
			{
				x64_sdaiSaveModelAsSimpleXmlBN(model, fileName);
			}
		}

		/// <summary>
		///		sdaiSaveModelAsSimpleXmlBNUnicode                       (https://rdf.bg/ifcdoc/CS64/sdaiSaveModelAsSimpleXmlBNUnicode.html)
		///
		///	This function saves the model as XML according to IFC4's way of XML serialization (wchar, i.e. Unicode file name).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsSimpleXmlBNUnicode")]
		public static extern void x86_sdaiSaveModelAsSimpleXmlBNUnicode(Int32 model, string fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsSimpleXmlBNUnicode")]
		public static extern void x64_sdaiSaveModelAsSimpleXmlBNUnicode(Int64 model, string fileName);

		public static void sdaiSaveModelAsSimpleXmlBNUnicode(Int64 model, string fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiSaveModelAsSimpleXmlBNUnicode((Int32)model, fileName);
			}
			else
			{
				x64_sdaiSaveModelAsSimpleXmlBNUnicode(model, fileName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsSimpleXmlBNUnicode")]
		public static extern void x86_sdaiSaveModelAsSimpleXmlBNUnicode(Int32 model, byte[] fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsSimpleXmlBNUnicode")]
		public static extern void x64_sdaiSaveModelAsSimpleXmlBNUnicode(Int64 model, byte[] fileName);

		public static void sdaiSaveModelAsSimpleXmlBNUnicode(Int64 model, byte[] fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiSaveModelAsSimpleXmlBNUnicode((Int32)model, fileName);
			}
			else
			{
				x64_sdaiSaveModelAsSimpleXmlBNUnicode(model, fileName);
			}
		}

		/// <summary>
		///		sdaiSaveModelAsJsonBN                                   (https://rdf.bg/ifcdoc/CS64/sdaiSaveModelAsJsonBN.html)
		///
		///	This function saves the model as JSON according to IFC4's way of JSON serialization (char file name).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsJsonBN")]
		public static extern void x86_sdaiSaveModelAsJsonBN(Int32 model, string fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsJsonBN")]
		public static extern void x64_sdaiSaveModelAsJsonBN(Int64 model, string fileName);

		public static void sdaiSaveModelAsJsonBN(Int64 model, string fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiSaveModelAsJsonBN((Int32)model, fileName);
			}
			else
			{
				x64_sdaiSaveModelAsJsonBN(model, fileName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsJsonBN")]
		public static extern void x86_sdaiSaveModelAsJsonBN(Int32 model, byte[] fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsJsonBN")]
		public static extern void x64_sdaiSaveModelAsJsonBN(Int64 model, byte[] fileName);

		public static void sdaiSaveModelAsJsonBN(Int64 model, byte[] fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiSaveModelAsJsonBN((Int32)model, fileName);
			}
			else
			{
				x64_sdaiSaveModelAsJsonBN(model, fileName);
			}
		}

		/// <summary>
		///		sdaiSaveModelAsJsonBNUnicode                            (https://rdf.bg/ifcdoc/CS64/sdaiSaveModelAsJsonBNUnicode.html)
		///
		///	This function saves the model as JSON according to IFC4's way of JSON serialization (wchar, i.e. Unicode file name).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsJsonBNUnicode")]
		public static extern void x86_sdaiSaveModelAsJsonBNUnicode(Int32 model, string fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsJsonBNUnicode")]
		public static extern void x64_sdaiSaveModelAsJsonBNUnicode(Int64 model, string fileName);

		public static void sdaiSaveModelAsJsonBNUnicode(Int64 model, string fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiSaveModelAsJsonBNUnicode((Int32)model, fileName);
			}
			else
			{
				x64_sdaiSaveModelAsJsonBNUnicode(model, fileName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsJsonBNUnicode")]
		public static extern void x86_sdaiSaveModelAsJsonBNUnicode(Int32 model, byte[] fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiSaveModelAsJsonBNUnicode")]
		public static extern void x64_sdaiSaveModelAsJsonBNUnicode(Int64 model, byte[] fileName);

		public static void sdaiSaveModelAsJsonBNUnicode(Int64 model, byte[] fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiSaveModelAsJsonBNUnicode((Int32)model, fileName);
			}
			else
			{
				x64_sdaiSaveModelAsJsonBNUnicode(model, fileName);
			}
		}

		/// <summary>
		///		engiSaveSchemaBN                                        (https://rdf.bg/ifcdoc/CS64/engiSaveSchemaBN.html)
		///
		///	This function saves the schema.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiSaveSchemaBN")]
		public static extern byte x86_engiSaveSchemaBN(Int32 model, string filePath);

		[DllImport(IFCEngineDLL, EntryPoint = "engiSaveSchemaBN")]
		public static extern byte x64_engiSaveSchemaBN(Int64 model, string filePath);

		public static byte engiSaveSchemaBN(Int64 model, string filePath)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiSaveSchemaBN((Int32)model, filePath);
				return _result;
			}
			else
			{
				return x64_engiSaveSchemaBN(model, filePath);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiSaveSchemaBN")]
		public static extern byte x86_engiSaveSchemaBN(Int32 model, byte[] filePath);

		[DllImport(IFCEngineDLL, EntryPoint = "engiSaveSchemaBN")]
		public static extern byte x64_engiSaveSchemaBN(Int64 model, byte[] filePath);

		public static byte engiSaveSchemaBN(Int64 model, byte[] filePath)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiSaveSchemaBN((Int32)model, filePath);
				return _result;
			}
			else
			{
				return x64_engiSaveSchemaBN(model, filePath);
			}
		}

		/// <summary>
		///		engiSaveSchemaBNUnicode                                 (https://rdf.bg/ifcdoc/CS64/engiSaveSchemaBNUnicode.html)
		///
		///	This function saves the schema (wchar, i.e. Unicode file name).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiSaveSchemaBNUnicode")]
		public static extern byte x86_engiSaveSchemaBNUnicode(Int32 model, string filePath);

		[DllImport(IFCEngineDLL, EntryPoint = "engiSaveSchemaBNUnicode")]
		public static extern byte x64_engiSaveSchemaBNUnicode(Int64 model, string filePath);

		public static byte engiSaveSchemaBNUnicode(Int64 model, string filePath)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiSaveSchemaBNUnicode((Int32)model, filePath);
				return _result;
			}
			else
			{
				return x64_engiSaveSchemaBNUnicode(model, filePath);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiSaveSchemaBNUnicode")]
		public static extern byte x86_engiSaveSchemaBNUnicode(Int32 model, byte[] filePath);

		[DllImport(IFCEngineDLL, EntryPoint = "engiSaveSchemaBNUnicode")]
		public static extern byte x64_engiSaveSchemaBNUnicode(Int64 model, byte[] filePath);

		public static byte engiSaveSchemaBNUnicode(Int64 model, byte[] filePath)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiSaveSchemaBNUnicode((Int32)model, filePath);
				return _result;
			}
			else
			{
				return x64_engiSaveSchemaBNUnicode(model, filePath);
			}
		}

		/// <summary>
		///		sdaiCloseModel                                          (https://rdf.bg/ifcdoc/CS64/sdaiCloseModel.html)
		///
		///	This function closes the model. After this call no instance handles will be available including all
		///	handles referencing the geometry of this specific file, in default compilation the model itself will
		///	be known in the kernel, however known to be disabled. Calls containing the model reference will be
		///	protected from crashing when called.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCloseModel")]
		public static extern void x86_sdaiCloseModel(Int32 model);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCloseModel")]
		public static extern void x64_sdaiCloseModel(Int64 model);

		public static void sdaiCloseModel(Int64 model)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiCloseModel((Int32)model);
			}
			else
			{
				x64_sdaiCloseModel(model);
			}
		}

		/// <summary>
		///		setPrecisionDoubleExport                                (https://rdf.bg/ifcdoc/CS64/setPrecisionDoubleExport.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "setPrecisionDoubleExport")]
		public static extern void x86_setPrecisionDoubleExport(Int32 model, Int32 precisionCap, Int32 precisionRound, bool clean);

		[DllImport(IFCEngineDLL, EntryPoint = "setPrecisionDoubleExport")]
		public static extern void x64_setPrecisionDoubleExport(Int64 model, Int64 precisionCap, Int64 precisionRound, bool clean);

		public static void setPrecisionDoubleExport(Int64 model, Int64 precisionCap, Int64 precisionRound, bool clean)
		{
			if (IntPtr.Size == 4)
			{
				x86_setPrecisionDoubleExport((Int32)model, (Int32)precisionCap, (Int32)precisionRound, clean);
			}
			else
			{
				x64_setPrecisionDoubleExport(model, precisionCap, precisionRound, clean);
			}
		}

        //
        //  Schema Reading API Calls
        //

		/// <summary>
		///		engiGetNextTypeDeclarationIterator                      (https://rdf.bg/ifcdoc/CS64/engiGetNextTypeDeclarationIterator.html)
		///
		///	This call returns next iterator of EXPRESS schema declarations for entities and types.
		///	If the input iterator is NULL it returns first iterator.
		///	If the input iterator is last it returns NULL.
		///	The declaration can be ENTITY, TYPE ENUM, TYPE SELECT, or defined TYPE.
		///	Use engiGetDeclarationFromIterator to access the further information.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetNextTypeDeclarationIterator")]
		public static extern Int32 x86_engiGetNextTypeDeclarationIterator(Int32 model, Int32 iterator);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetNextTypeDeclarationIterator")]
		public static extern Int64 x64_engiGetNextTypeDeclarationIterator(Int64 model, Int64 iterator);

		public static Int64 engiGetNextTypeDeclarationIterator(Int64 model, Int64 iterator)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetNextTypeDeclarationIterator((Int32)model, (Int32)iterator);
				return _result;
			}
			else
			{
				return x64_engiGetNextTypeDeclarationIterator(model, iterator);
			}
		}

		/// <summary>
		///		engiGetTypeDeclarationFromIterator                      (https://rdf.bg/ifcdoc/CS64/engiGetTypeDeclarationFromIterator.html)
		///
		///	This call returns handle to the EXPRESS schema declaration from iterator.
		///	The declaration can be ENTITY, TYPE ENUM, TYPE SELECT, or defined TYPE.
		///	Use engiGetDeclarationType to access the further information.
		///	Use engiGetNextTypeDeclarationIterator to iterate declarations.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetTypeDeclarationFromIterator")]
		public static extern Int32 x86_engiGetTypeDeclarationFromIterator(Int32 model, Int32 iterator);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetTypeDeclarationFromIterator")]
		public static extern Int64 x64_engiGetTypeDeclarationFromIterator(Int64 model, Int64 iterator);

		public static Int64 engiGetTypeDeclarationFromIterator(Int64 model, Int64 iterator)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetTypeDeclarationFromIterator((Int32)model, (Int32)iterator);
				return _result;
			}
			else
			{
				return x64_engiGetTypeDeclarationFromIterator(model, iterator);
			}
		}

		/// <summary>
		///		engiGetSchemaScriptDeclarationByIterator                (https://rdf.bg/ifcdoc/CS64/engiGetSchemaScriptDeclarationByIterator.html)
		///
		///	This call iterates EXPRESS schema declarations of FUNCTION, PROCEDURE or RULE.
		///	If prev is NULL it returns first declaration of above kinds.
		///	If prev is the last declaration it returns NULL.
		///	Use engiGetDeclarationType to access the further information.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetSchemaScriptDeclarationByIterator")]
		public static extern Int32 x86_engiGetSchemaScriptDeclarationByIterator(Int32 model, Int32 prev);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetSchemaScriptDeclarationByIterator")]
		public static extern Int64 x64_engiGetSchemaScriptDeclarationByIterator(Int64 model, Int64 prev);

		public static Int64 engiGetSchemaScriptDeclarationByIterator(Int64 model, Int64 prev)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetSchemaScriptDeclarationByIterator((Int32)model, (Int32)prev);
				return _result;
			}
			else
			{
				return x64_engiGetSchemaScriptDeclarationByIterator(model, prev);
			}
		}

		/// <summary>
		///		engiGetDeclarationType                                  (https://rdf.bg/ifcdoc/CS64/engiGetDeclarationType.html)
		///
		///	This call returns a type of the EXPRESS schema declarations from its handle.
		///
		///	The following functions can be used to get further information
		///		ENTITY: this SchemaDecl can be casted to SdaiEntity and used in engiGetEntityName and any other entity inquiry function
		///		TYPE ENUM: engiGetEnumerationElement
		///		TYPE SELECT: engiGetSelectElement
		///		DEFINED_TYPE: engiGetDefinedType
		///		FUNCTION, PROCEDURE, RULE, WHERE_RULE: engiGetScriptText
		///
		///	Use engiGetTypeDeclarationFromIterator or engiGetSchemaScriptDeclarationByIterator to obtain declaration handle.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetDeclarationType")]
		public static extern enum_express_declaration x86_engiGetDeclarationType(Int32 declaration);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetDeclarationType")]
		public static extern enum_express_declaration x64_engiGetDeclarationType(Int64 declaration);

		public static enum_express_declaration engiGetDeclarationType(Int64 declaration)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetDeclarationType((Int32)declaration);
				return _result;
			}
			else
			{
				return x64_engiGetDeclarationType(declaration);
			}
		}

		/// <summary>
		///		engiGetEnumerationElement                               (https://rdf.bg/ifcdoc/CS64/engiGetEnumerationElement.html)
		///
		///	This call returns a name of the enumeration element with the given index (zero based).
		///	It returns NULL if the index out of range.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEnumerationElement")]
		public static extern IntPtr x86_engiGetEnumerationElement(Int32 enumeration, Int32 index);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEnumerationElement")]
		public static extern IntPtr x64_engiGetEnumerationElement(Int64 enumeration, Int64 index);

		public static IntPtr engiGetEnumerationElement(Int64 enumeration, Int64 index)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEnumerationElement((Int32)enumeration, (Int32)index);
				return _result;
			}
			else
			{
				return x64_engiGetEnumerationElement(enumeration, index);
			}
		}

		/// <summary>
		///		engiGetSelectElement                                    (https://rdf.bg/ifcdoc/CS64/engiGetSelectElement.html)
		///
		///	This call returns a declaration handle of the select element with the given index (zero based).
		///	It returns 0 if the index out of range.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetSelectElement")]
		public static extern Int32 x86_engiGetSelectElement(Int32 select, Int32 index);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetSelectElement")]
		public static extern Int64 x64_engiGetSelectElement(Int64 select, Int64 index);

		public static Int64 engiGetSelectElement(Int64 select, Int64 index)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetSelectElement((Int32)select, (Int32)index);
				return _result;
			}
			else
			{
				return x64_engiGetSelectElement(select, index);
			}
		}

		/// <summary>
		///		engiGetDefinedType                                      (https://rdf.bg/ifcdoc/CS64/engiGetDefinedType.html)
		///
		///	This call returns a simple type for defined type handle and can inquire referenced type, if any.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetDefinedType")]
		public static extern enum_express_attr_type x86_engiGetDefinedType(Int32 definedType, Int32 referencedDeclaration, out Int32 aggregationDefinition);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetDefinedType")]
		public static extern enum_express_attr_type x64_engiGetDefinedType(Int64 definedType, Int64 referencedDeclaration, out Int64 aggregationDefinition);

		public static enum_express_attr_type engiGetDefinedType(Int64 definedType, Int64 referencedDeclaration, out Int64 aggregationDefinition)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetDefinedType((Int32)definedType, (Int32)referencedDeclaration, out Int32 _aggregationDefinition);
				aggregationDefinition = _aggregationDefinition;
				return _result;
			}
			else
			{
				return x64_engiGetDefinedType(definedType, referencedDeclaration, out aggregationDefinition);
			}
		}

		/// <summary>
		///		engiGetScriptText                                       (https://rdf.bg/ifcdoc/CS64/engiGetScriptText.html)
		///
		///	This call returns name and body text for entity local (where) rule, schema rule, function or procedure.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetScriptText")]
		public static extern void x86_engiGetScriptText(Int32 declaration, out IntPtr label, out IntPtr text);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetScriptText")]
		public static extern void x64_engiGetScriptText(Int64 declaration, out IntPtr label, out IntPtr text);

		public static void engiGetScriptText(Int64 declaration, out IntPtr label, out IntPtr text)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiGetScriptText((Int32)declaration, out IntPtr _label, out IntPtr _text);
				label = _label;
				text = _text;
			}
			else
			{
				x64_engiGetScriptText(declaration, out label, out text);
			}
		}

		/// <summary>
		///		engiEvaluateScriptExpression                            (https://rdf.bg/ifcdoc/CS64/engiEvaluateScriptExpression.html)
		///
		///	This function can evaluate EXPRESS expression for entity where rule or derived attribute,
		///	valueType, value and return type work similary to sdaiGetAttr.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiEvaluateScriptExpression")]
		public static extern Int32 x86_engiEvaluateScriptExpression(Int32 model, Int32 instance, Int32 expression, Int32 valueType, out bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "engiEvaluateScriptExpression")]
		public static extern Int64 x64_engiEvaluateScriptExpression(Int64 model, Int64 instance, Int64 expression, Int64 valueType, out bool value);

		public static Int64 engiEvaluateScriptExpression(Int64 model, Int64 instance, Int64 expression, Int64 valueType, out bool value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiEvaluateScriptExpression((Int32)model, (Int32)instance, (Int32)expression, (Int32)valueType, out bool _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_engiEvaluateScriptExpression(model, instance, expression, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiEvaluateScriptExpression")]
		public static extern Int32 x86_engiEvaluateScriptExpression(Int32 model, Int32 instance, Int32 expression, Int32 valueType, out Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "engiEvaluateScriptExpression")]
		public static extern Int64 x64_engiEvaluateScriptExpression(Int64 model, Int64 instance, Int64 expression, Int64 valueType, out Int64 value);

		public static Int64 engiEvaluateScriptExpression(Int64 model, Int64 instance, Int64 expression, Int64 valueType, out Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiEvaluateScriptExpression((Int32)model, (Int32)instance, (Int32)expression, (Int32)valueType, out Int32 _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_engiEvaluateScriptExpression(model, instance, expression, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiEvaluateScriptExpression")]
		public static extern Int32 x86_engiEvaluateScriptExpression(Int32 model, Int32 instance, Int32 expression, Int32 valueType, out double value);

		[DllImport(IFCEngineDLL, EntryPoint = "engiEvaluateScriptExpression")]
		public static extern Int64 x64_engiEvaluateScriptExpression(Int64 model, Int64 instance, Int64 expression, Int64 valueType, out double value);

		public static Int64 engiEvaluateScriptExpression(Int64 model, Int64 instance, Int64 expression, Int64 valueType, out double value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiEvaluateScriptExpression((Int32)model, (Int32)instance, (Int32)expression, (Int32)valueType, out double _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_engiEvaluateScriptExpression(model, instance, expression, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiEvaluateScriptExpression")]
		public static extern Int32 x86_engiEvaluateScriptExpression(Int32 model, Int32 instance, Int32 expression, Int32 valueType, out IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "engiEvaluateScriptExpression")]
		public static extern Int64 x64_engiEvaluateScriptExpression(Int64 model, Int64 instance, Int64 expression, Int64 valueType, out IntPtr value);

		public static Int64 engiEvaluateScriptExpression(Int64 model, Int64 instance, Int64 expression, Int64 valueType, out IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiEvaluateScriptExpression((Int32)model, (Int32)instance, (Int32)expression, (Int32)valueType, out IntPtr _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_engiEvaluateScriptExpression(model, instance, expression, valueType, out value);
			}
		}

		/// <summary>
		///		sdaiGetEntity                                           (https://rdf.bg/ifcdoc/CS64/sdaiGetEntity.html)
		///
		///	This call retrieves a handle to an entity based on a given entity name.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetEntity")]
		public static extern Int32 x86_sdaiGetEntity(Int32 model, string entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetEntity")]
		public static extern Int64 x64_sdaiGetEntity(Int64 model, string entityName);

		public static Int64 sdaiGetEntity(Int64 model, string entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetEntity((Int32)model, entityName);
				return _result;
			}
			else
			{
				return x64_sdaiGetEntity(model, entityName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetEntity")]
		public static extern Int32 x86_sdaiGetEntity(Int32 model, byte[] entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetEntity")]
		public static extern Int64 x64_sdaiGetEntity(Int64 model, byte[] entityName);

		public static Int64 sdaiGetEntity(Int64 model, byte[] entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetEntity((Int32)model, entityName);
				return _result;
			}
			else
			{
				return x64_sdaiGetEntity(model, entityName);
			}
		}

		/// <summary>
		///		sdaiGetComplexEntity                                    (https://rdf.bg/ifcdoc/CS64/sdaiGetComplexEntity.html)
		///
		///	This call retrieves a handle to an entity composed of the supplied simple entity types.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetComplexEntity")]
		public static extern Int32 x86_sdaiGetComplexEntity(Int32 model, Int32 entityList);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetComplexEntity")]
		public static extern Int64 x64_sdaiGetComplexEntity(Int64 model, Int64 entityList);

		public static Int64 sdaiGetComplexEntity(Int64 model, Int64 entityList)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetComplexEntity((Int32)model, (Int32)entityList);
				return _result;
			}
			else
			{
				return x64_sdaiGetComplexEntity(model, entityList);
			}
		}

		/// <summary>
		///		sdaiGetComplexEntityBN                                  (https://rdf.bg/ifcdoc/CS64/sdaiGetComplexEntityBN.html)
		///
		///	This call retrieves a handle to an entity composed of the supplied simple entity types.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetComplexEntityBN")]
		public static extern Int32 x86_sdaiGetComplexEntityBN(Int32 model, Int32 nameNumber, out IntPtr nameVector);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetComplexEntityBN")]
		public static extern Int64 x64_sdaiGetComplexEntityBN(Int64 model, Int64 nameNumber, out IntPtr nameVector);

		public static Int64 sdaiGetComplexEntityBN(Int64 model, Int64 nameNumber, out IntPtr nameVector)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetComplexEntityBN((Int32)model, (Int32)nameNumber, out IntPtr _nameVector);
				nameVector = _nameVector;
				return _result;
			}
			else
			{
				return x64_sdaiGetComplexEntityBN(model, nameNumber, out nameVector);
			}
		}

		/// <summary>
		///		engiGetEntityModel                                      (https://rdf.bg/ifcdoc/CS64/engiGetEntityModel.html)
		///
		///	This call retrieves a model based on a given entity handle.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityModel")]
		public static extern Int32 x86_engiGetEntityModel(Int32 entity);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityModel")]
		public static extern Int64 x64_engiGetEntityModel(Int64 entity);

		public static Int64 engiGetEntityModel(Int64 entity)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityModel((Int32)entity);
				return _result;
			}
			else
			{
				return x64_engiGetEntityModel(entity);
			}
		}

		/// <summary>
		///		engiGetAttrIndex                                        (https://rdf.bg/ifcdoc/CS64/engiGetAttrIndex.html)
		///
		///	This call works for non-complex entities and entities without multiple inheritance,
		///	it is advised not to use this call for other schemas.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrIndex")]
		public static extern Int32 x86_engiGetAttrIndex(Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrIndex")]
		public static extern Int64 x64_engiGetAttrIndex(Int64 attribute);

		public static Int64 engiGetAttrIndex(Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrIndex((Int32)attribute);
				return _result;
			}
			else
			{
				return x64_engiGetAttrIndex(attribute);
			}
		}

		/// <summary>
		///		engiGetAttrIndexBN                                      (https://rdf.bg/ifcdoc/CS64/engiGetAttrIndexBN.html)
		///
		///	This call works for non-complex entities and entities without multiple inheritance,
		///	it is advised not to use this call for other schemas.
		///
		///	Technically engiGetAttrIndexBN will transform into the following call
		///		engiGetAttrIndex(
		///				sdaiGetAttrDefinition(
		///						entity,
		///						attributeName
		///					)
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrIndexBN")]
		public static extern Int32 x86_engiGetAttrIndexBN(Int32 entity, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrIndexBN")]
		public static extern Int64 x64_engiGetAttrIndexBN(Int64 entity, string attributeName);

		public static Int64 engiGetAttrIndexBN(Int64 entity, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrIndexBN((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiGetAttrIndexBN(entity, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrIndexBN")]
		public static extern Int32 x86_engiGetAttrIndexBN(Int32 entity, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrIndexBN")]
		public static extern Int64 x64_engiGetAttrIndexBN(Int64 entity, byte[] attributeName);

		public static Int64 engiGetAttrIndexBN(Int64 entity, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrIndexBN((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiGetAttrIndexBN(entity, attributeName);
			}
		}

		/// <summary>
		///		engiGetAttrIndexEx                                      (https://rdf.bg/ifcdoc/CS64/engiGetAttrIndexEx.html)
		///
		///	This call works for non-complex entities and entities without multiple inheritance,
		///	it is advised not to use this call for other schemas.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrIndexEx")]
		public static extern Int32 x86_engiGetAttrIndexEx(Int32 attribute, bool countedWithParents, bool countedWithInverse);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrIndexEx")]
		public static extern Int64 x64_engiGetAttrIndexEx(Int64 attribute, bool countedWithParents, bool countedWithInverse);

		public static Int64 engiGetAttrIndexEx(Int64 attribute, bool countedWithParents, bool countedWithInverse)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrIndexEx((Int32)attribute, countedWithParents, countedWithInverse);
				return _result;
			}
			else
			{
				return x64_engiGetAttrIndexEx(attribute, countedWithParents, countedWithInverse);
			}
		}

		/// <summary>
		///		engiGetAttrIndexExBN                                    (https://rdf.bg/ifcdoc/CS64/engiGetAttrIndexExBN.html)
		///
		///	This call works for non-complex entities and entities without multiple inheritance,
		///	it is advised not to use this call for other schemas.
		///
		///	Technically engiGetAttrIndexExBN will transform into the following call
		///		engiGetAttrIndexEx(
		///				sdaiGetAttrDefinition(
		///						entity,
		///						attributeName
		///					),
		///				countedWithParents,
		///				countedWithInverse
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrIndexExBN")]
		public static extern Int32 x86_engiGetAttrIndexExBN(Int32 entity, string attributeName, bool countedWithParents, bool countedWithInverse);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrIndexExBN")]
		public static extern Int64 x64_engiGetAttrIndexExBN(Int64 entity, string attributeName, bool countedWithParents, bool countedWithInverse);

		public static Int64 engiGetAttrIndexExBN(Int64 entity, string attributeName, bool countedWithParents, bool countedWithInverse)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrIndexExBN((Int32)entity, attributeName, countedWithParents, countedWithInverse);
				return _result;
			}
			else
			{
				return x64_engiGetAttrIndexExBN(entity, attributeName, countedWithParents, countedWithInverse);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrIndexExBN")]
		public static extern Int32 x86_engiGetAttrIndexExBN(Int32 entity, byte[] attributeName, bool countedWithParents, bool countedWithInverse);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrIndexExBN")]
		public static extern Int64 x64_engiGetAttrIndexExBN(Int64 entity, byte[] attributeName, bool countedWithParents, bool countedWithInverse);

		public static Int64 engiGetAttrIndexExBN(Int64 entity, byte[] attributeName, bool countedWithParents, bool countedWithInverse)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrIndexExBN((Int32)entity, attributeName, countedWithParents, countedWithInverse);
				return _result;
			}
			else
			{
				return x64_engiGetAttrIndexExBN(entity, attributeName, countedWithParents, countedWithInverse);
			}
		}

		/// <summary>
		///		engiGetAttrNameByIndex                                  (https://rdf.bg/ifcdoc/CS64/engiGetAttrNameByIndex.html)
		///
		///	This call can be used to retrieve the name of the n-th argument of the given entity. Arguments of parent entities are included in the index. Both explicit and inverse attributes are included.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrNameByIndex")]
		public static extern IntPtr x86_engiGetAttrNameByIndex(Int32 entity, Int32 index, Int32 valueType, out IntPtr attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrNameByIndex")]
		public static extern IntPtr x64_engiGetAttrNameByIndex(Int64 entity, Int64 index, Int64 valueType, out IntPtr attributeName);

		public static IntPtr engiGetAttrNameByIndex(Int64 entity, Int64 index, Int64 valueType, out IntPtr attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrNameByIndex((Int32)entity, (Int32)index, (Int32)valueType, out IntPtr _attributeName);
				attributeName = _attributeName;
				return _result;
			}
			else
			{
				return x64_engiGetAttrNameByIndex(entity, index, valueType, out attributeName);
			}
		}

		/// <summary>
		///		engiGetAttrTypeByIndex                                  (https://rdf.bg/ifcdoc/CS64/engiGetAttrTypeByIndex.html)
		///
		///	This call can be used to retrieve the type of the n-th argument of the given entity. In case of a select argument no relevant information is given by this call as it depends on the instance.
		///	Arguments of parent entities are included in the index. Both explicit and inverse attributes are included.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrTypeByIndex")]
		public static extern void x86_engiGetAttrTypeByIndex(Int32 entity, Int32 index, out Int32 attributeType);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrTypeByIndex")]
		public static extern void x64_engiGetAttrTypeByIndex(Int64 entity, Int64 index, out Int64 attributeType);

		public static void engiGetAttrTypeByIndex(Int64 entity, Int64 index, out Int64 attributeType)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiGetAttrTypeByIndex((Int32)entity, (Int32)index, out Int32 _attributeType);
				attributeType = _attributeType;
			}
			else
			{
				x64_engiGetAttrTypeByIndex(entity, index, out attributeType);
			}
		}

		/// <summary>
		///		engiGetEntityCount                                      (https://rdf.bg/ifcdoc/CS64/engiGetEntityCount.html)
		///
		///	Returns the total number of entities within the loaded schema.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityCount")]
		public static extern Int32 x86_engiGetEntityCount(Int32 model);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityCount")]
		public static extern Int64 x64_engiGetEntityCount(Int64 model);

		public static Int64 engiGetEntityCount(Int64 model)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityCount((Int32)model);
				return _result;
			}
			else
			{
				return x64_engiGetEntityCount(model);
			}
		}

		/// <summary>
		///		engiGetEntityElement                                    (https://rdf.bg/ifcdoc/CS64/engiGetEntityElement.html)
		///
		///	This call returns a specific entity based on an index, the index needs to be 0 or higher but lower then the number of entities in the loaded schema.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityElement")]
		public static extern Int32 x86_engiGetEntityElement(Int32 model, Int32 index);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityElement")]
		public static extern Int64 x64_engiGetEntityElement(Int64 model, Int64 index);

		public static Int64 engiGetEntityElement(Int64 model, Int64 index)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityElement((Int32)model, (Int32)index);
				return _result;
			}
			else
			{
				return x64_engiGetEntityElement(model, index);
			}
		}

		/// <summary>
		///		sdaiGetEntityExtent                                     (https://rdf.bg/ifcdoc/CS64/sdaiGetEntityExtent.html)
		///
		///	This call retrieves an aggregation that contains all instances of the entity given.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetEntityExtent")]
		public static extern Int32 x86_sdaiGetEntityExtent(Int32 model, Int32 entity);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetEntityExtent")]
		public static extern Int64 x64_sdaiGetEntityExtent(Int64 model, Int64 entity);

		public static Int64 sdaiGetEntityExtent(Int64 model, Int64 entity)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetEntityExtent((Int32)model, (Int32)entity);
				return _result;
			}
			else
			{
				return x64_sdaiGetEntityExtent(model, entity);
			}
		}

		/// <summary>
		///		sdaiGetEntityExtentBN                                   (https://rdf.bg/ifcdoc/CS64/sdaiGetEntityExtentBN.html)
		///
		///	This call retrieves an aggregation that contains all instances of the entity given.
		///
		///	Technically sdaiGetEntityExtentBN will transform into the following call
		///		sdaiGetEntityExtent(
		///				model,
		///				sdaiGetEntity(
		///						model,
		///						entityName
		///					)
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetEntityExtentBN")]
		public static extern Int32 x86_sdaiGetEntityExtentBN(Int32 model, string entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetEntityExtentBN")]
		public static extern Int64 x64_sdaiGetEntityExtentBN(Int64 model, string entityName);

		public static Int64 sdaiGetEntityExtentBN(Int64 model, string entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetEntityExtentBN((Int32)model, entityName);
				return _result;
			}
			else
			{
				return x64_sdaiGetEntityExtentBN(model, entityName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetEntityExtentBN")]
		public static extern Int32 x86_sdaiGetEntityExtentBN(Int32 model, byte[] entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetEntityExtentBN")]
		public static extern Int64 x64_sdaiGetEntityExtentBN(Int64 model, byte[] entityName);

		public static Int64 sdaiGetEntityExtentBN(Int64 model, byte[] entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetEntityExtentBN((Int32)model, entityName);
				return _result;
			}
			else
			{
				return x64_sdaiGetEntityExtentBN(model, entityName);
			}
		}

		/// <summary>
		///		engiGetEntityName                                       (https://rdf.bg/ifcdoc/CS64/engiGetEntityName.html)
		///
		///	This call can be used to get the name of the given entity.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityName")]
		public static extern IntPtr x86_engiGetEntityName(Int32 entity, Int32 valueType, out IntPtr entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityName")]
		public static extern IntPtr x64_engiGetEntityName(Int64 entity, Int64 valueType, out IntPtr entityName);

		public static IntPtr engiGetEntityName(Int64 entity, Int64 valueType, out IntPtr entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityName((Int32)entity, (Int32)valueType, out IntPtr _entityName);
				entityName = _entityName;
				return _result;
			}
			else
			{
				return x64_engiGetEntityName(entity, valueType, out entityName);
			}
		}

		/// <summary>
		///		engiGetEntityNoAttributes                               (https://rdf.bg/ifcdoc/CS64/engiGetEntityNoAttributes.html)
		///
		///	This call returns the number of arguments, this includes the arguments of its (nested) parents and inverse arguments.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityNoAttributes")]
		public static extern Int32 x86_engiGetEntityNoAttributes(Int32 entity);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityNoAttributes")]
		public static extern Int64 x64_engiGetEntityNoAttributes(Int64 entity);

		public static Int64 engiGetEntityNoAttributes(Int64 entity)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityNoAttributes((Int32)entity);
				return _result;
			}
			else
			{
				return x64_engiGetEntityNoAttributes(entity);
			}
		}

		/// <summary>
		///		engiGetEntityNoAttributesEx                             (https://rdf.bg/ifcdoc/CS64/engiGetEntityNoAttributesEx.html)
		///
		///	This call returns the number of attributes, inclusion of parents and inverse depends on includeParent and includeInverse values.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityNoAttributesEx")]
		public static extern Int32 x86_engiGetEntityNoAttributesEx(Int32 entity, bool includeParent, bool includeInverse);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityNoAttributesEx")]
		public static extern Int64 x64_engiGetEntityNoAttributesEx(Int64 entity, bool includeParent, bool includeInverse);

		public static Int64 engiGetEntityNoAttributesEx(Int64 entity, bool includeParent, bool includeInverse)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityNoAttributesEx((Int32)entity, includeParent, includeInverse);
				return _result;
			}
			else
			{
				return x64_engiGetEntityNoAttributesEx(entity, includeParent, includeInverse);
			}
		}

		/// <summary>
		///		engiGetEntityParent                                     (https://rdf.bg/ifcdoc/CS64/engiGetEntityParent.html)
		///
		///	Returns the first parent entity, for example the parent of IfcObject is IfcObjectDefinition, of IfcObjectDefinition is IfcRoot and of IfcRoot is 0.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityParent")]
		public static extern Int32 x86_engiGetEntityParent(Int32 entity);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityParent")]
		public static extern Int64 x64_engiGetEntityParent(Int64 entity);

		public static Int64 engiGetEntityParent(Int64 entity)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityParent((Int32)entity);
				return _result;
			}
			else
			{
				return x64_engiGetEntityParent(entity);
			}
		}

		/// <summary>
		///		engiGetEntityNoParents                                  (https://rdf.bg/ifcdoc/CS64/engiGetEntityNoParents.html)
		///
		///	Returns number of parent entities.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityNoParents")]
		public static extern Int32 x86_engiGetEntityNoParents(Int32 entity);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityNoParents")]
		public static extern Int64 x64_engiGetEntityNoParents(Int64 entity);

		public static Int64 engiGetEntityNoParents(Int64 entity)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityNoParents((Int32)entity);
				return _result;
			}
			else
			{
				return x64_engiGetEntityNoParents(entity);
			}
		}

		/// <summary>
		///		engiGetEntityParentEx                                   (https://rdf.bg/ifcdoc/CS64/engiGetEntityParentEx.html)
		///
		///	Returns the N-th parent of entity or NULL if index exceeds number of parents.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityParentEx")]
		public static extern Int32 x86_engiGetEntityParentEx(Int32 entity, Int32 index);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityParentEx")]
		public static extern Int64 x64_engiGetEntityParentEx(Int64 entity, Int64 index);

		public static Int64 engiGetEntityParentEx(Int64 entity, Int64 index)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityParentEx((Int32)entity, (Int32)index);
				return _result;
			}
			else
			{
				return x64_engiGetEntityParentEx(entity, index);
			}
		}

		/// <summary>
		///		engiGetAttrDerived                                      (https://rdf.bg/ifcdoc/CS64/engiGetAttrDerived.html)
		///
		///	This call can be used to check if an attribute is defined schema wise in the context of a certain entity.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDerived")]
		public static extern Int32 x86_engiGetAttrDerived(Int32 entity, Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDerived")]
		public static extern Int64 x64_engiGetAttrDerived(Int64 entity, Int64 attribute);

		public static Int64 engiGetAttrDerived(Int64 entity, Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrDerived((Int32)entity, (Int32)attribute);
				return _result;
			}
			else
			{
				return x64_engiGetAttrDerived(entity, attribute);
			}
		}

		/// <summary>
		///		engiGetAttrDerivedBN                                    (https://rdf.bg/ifcdoc/CS64/engiGetAttrDerivedBN.html)
		///
		///	This call can be used to check if an attribute is defined schema wise in the context of a certain entity.
		///
		///	Technically engiGetAttrDerivedBN will transform into the following call
		///		engiGetAttrDerived(
		///				entity,
		///				sdaiGetAttrDefinition(
		///						entity,
		///						attributeName
		///					)
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDerivedBN")]
		public static extern Int32 x86_engiGetAttrDerivedBN(Int32 entity, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDerivedBN")]
		public static extern Int64 x64_engiGetAttrDerivedBN(Int64 entity, string attributeName);

		public static Int64 engiGetAttrDerivedBN(Int64 entity, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrDerivedBN((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiGetAttrDerivedBN(entity, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDerivedBN")]
		public static extern Int32 x86_engiGetAttrDerivedBN(Int32 entity, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDerivedBN")]
		public static extern Int64 x64_engiGetAttrDerivedBN(Int64 entity, byte[] attributeName);

		public static Int64 engiGetAttrDerivedBN(Int64 entity, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrDerivedBN((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiGetAttrDerivedBN(entity, attributeName);
			}
		}

		/// <summary>
		///		engiIsAttrInverse                                       (https://rdf.bg/ifcdoc/CS64/engiIsAttrInverse.html)
		///
		///	This call can be used to check if an attribute is an inverse relation
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrInverse")]
		public static extern bool x86_engiIsAttrInverse(Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrInverse")]
		public static extern bool x64_engiIsAttrInverse(Int64 attribute);

		public static bool engiIsAttrInverse(Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiIsAttrInverse((Int32)attribute);
				return _result;
			}
			else
			{
				return x64_engiIsAttrInverse(attribute);
			}
		}

		/// <summary>
		///		engiIsAttrInverseBN                                     (https://rdf.bg/ifcdoc/CS64/engiIsAttrInverseBN.html)
		///
		///	This call can be used to check if an attribute is an inverse relation.
		///
		///	Technically engiIsAttrInverseBN will transform into the following call
		///		engiIsAttrInverse(
		///				sdaiGetAttrDefinition(
		///						entity,
		///						attributeName
		///					)
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrInverseBN")]
		public static extern bool x86_engiIsAttrInverseBN(Int32 entity, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrInverseBN")]
		public static extern bool x64_engiIsAttrInverseBN(Int64 entity, string attributeName);

		public static bool engiIsAttrInverseBN(Int64 entity, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiIsAttrInverseBN((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiIsAttrInverseBN(entity, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrInverseBN")]
		public static extern bool x86_engiIsAttrInverseBN(Int32 entity, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrInverseBN")]
		public static extern bool x64_engiIsAttrInverseBN(Int64 entity, byte[] attributeName);

		public static bool engiIsAttrInverseBN(Int64 entity, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiIsAttrInverseBN((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiIsAttrInverseBN(entity, attributeName);
			}
		}

		/// <summary>
		///		engiIsAttrOptional                                      (https://rdf.bg/ifcdoc/CS64/engiIsAttrOptional.html)
		///
		///	This call can be used to check if an attribute is optional.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrOptional")]
		public static extern bool x86_engiIsAttrOptional(Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrOptional")]
		public static extern bool x64_engiIsAttrOptional(Int64 attribute);

		public static bool engiIsAttrOptional(Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiIsAttrOptional((Int32)attribute);
				return _result;
			}
			else
			{
				return x64_engiIsAttrOptional(attribute);
			}
		}

		/// <summary>
		///		engiIsAttrOptionalBN                                    (https://rdf.bg/ifcdoc/CS64/engiIsAttrOptionalBN.html)
		///
		///	This call can be used to check if an attribute is optional.
		///
		///	Technically engiIsAttrOptionalBN will transform into the following call
		///		engiIsAttrOptional(
		///				sdaiGetAttrDefinition(
		///						entity,
		///						attributeName
		///					)
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrOptionalBN")]
		public static extern bool x86_engiIsAttrOptionalBN(Int32 entity, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrOptionalBN")]
		public static extern bool x64_engiIsAttrOptionalBN(Int64 entity, string attributeName);

		public static bool engiIsAttrOptionalBN(Int64 entity, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiIsAttrOptionalBN((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiIsAttrOptionalBN(entity, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrOptionalBN")]
		public static extern bool x86_engiIsAttrOptionalBN(Int32 entity, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrOptionalBN")]
		public static extern bool x64_engiIsAttrOptionalBN(Int64 entity, byte[] attributeName);

		public static bool engiIsAttrOptionalBN(Int64 entity, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiIsAttrOptionalBN((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiIsAttrOptionalBN(entity, attributeName);
			}
		}

		/// <summary>
		///		engiGetAttrDomainName                                   (https://rdf.bg/ifcdoc/CS64/engiGetAttrDomainName.html)
		///
		///	This call can be used to get the domain of an attribute.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDomainName")]
		public static extern IntPtr x86_engiGetAttrDomainName(Int32 attribute, out IntPtr domainName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDomainName")]
		public static extern IntPtr x64_engiGetAttrDomainName(Int64 attribute, out IntPtr domainName);

		public static IntPtr engiGetAttrDomainName(Int64 attribute, out IntPtr domainName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrDomainName((Int32)attribute, out IntPtr _domainName);
				domainName = _domainName;
				return _result;
			}
			else
			{
				return x64_engiGetAttrDomainName(attribute, out domainName);
			}
		}

		/// <summary>
		///		engiGetAttrDomainNameBN                                 (https://rdf.bg/ifcdoc/CS64/engiGetAttrDomainNameBN.html)
		///
		///	This call can be used to get the domain of an attribute.
		///
		///	Technically engiGetAttrDomainNameBN will transform into the following call
		///		engiGetAttrDomainName(
		///				sdaiGetAttrDefinition(
		///						entity,
		///						attributeName
		///					),
		///				domainName
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDomainNameBN")]
		public static extern IntPtr x86_engiGetAttrDomainNameBN(Int32 entity, string attributeName, out IntPtr domainName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDomainNameBN")]
		public static extern IntPtr x64_engiGetAttrDomainNameBN(Int64 entity, string attributeName, out IntPtr domainName);

		public static IntPtr engiGetAttrDomainNameBN(Int64 entity, string attributeName, out IntPtr domainName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrDomainNameBN((Int32)entity, attributeName, out IntPtr _domainName);
				domainName = _domainName;
				return _result;
			}
			else
			{
				return x64_engiGetAttrDomainNameBN(entity, attributeName, out domainName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDomainNameBN")]
		public static extern IntPtr x86_engiGetAttrDomainNameBN(Int32 entity, byte[] attributeName, out IntPtr domainName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDomainNameBN")]
		public static extern IntPtr x64_engiGetAttrDomainNameBN(Int64 entity, byte[] attributeName, out IntPtr domainName);

		public static IntPtr engiGetAttrDomainNameBN(Int64 entity, byte[] attributeName, out IntPtr domainName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrDomainNameBN((Int32)entity, attributeName, out IntPtr _domainName);
				domainName = _domainName;
				return _result;
			}
			else
			{
				return x64_engiGetAttrDomainNameBN(entity, attributeName, out domainName);
			}
		}

		/// <summary>
		///		engiIsEntityAbstract                                    (https://rdf.bg/ifcdoc/CS64/engiIsEntityAbstract.html)
		///
		///	This call can be used to check if an entity is abstract.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiIsEntityAbstract")]
		public static extern Int32 x86_engiIsEntityAbstract(Int32 entity);

		[DllImport(IFCEngineDLL, EntryPoint = "engiIsEntityAbstract")]
		public static extern Int64 x64_engiIsEntityAbstract(Int64 entity);

		public static Int64 engiIsEntityAbstract(Int64 entity)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiIsEntityAbstract((Int32)entity);
				return _result;
			}
			else
			{
				return x64_engiIsEntityAbstract(entity);
			}
		}

		/// <summary>
		///		engiIsEntityAbstractBN                                  (https://rdf.bg/ifcdoc/CS64/engiIsEntityAbstractBN.html)
		///
		///	This call can be used to check if an entity is abstract.
		///
		///	Technically engiIsEntityAbstractBN will transform into the following call
		///		engiIsEntityAbstract(
		///				sdaiGetEntity(
		///						model,
		///						entityName
		///					)
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiIsEntityAbstractBN")]
		public static extern Int32 x86_engiIsEntityAbstractBN(Int32 model, string entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiIsEntityAbstractBN")]
		public static extern Int64 x64_engiIsEntityAbstractBN(Int64 model, string entityName);

		public static Int64 engiIsEntityAbstractBN(Int64 model, string entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiIsEntityAbstractBN((Int32)model, entityName);
				return _result;
			}
			else
			{
				return x64_engiIsEntityAbstractBN(model, entityName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiIsEntityAbstractBN")]
		public static extern Int32 x86_engiIsEntityAbstractBN(Int32 model, byte[] entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiIsEntityAbstractBN")]
		public static extern Int64 x64_engiIsEntityAbstractBN(Int64 model, byte[] entityName);

		public static Int64 engiIsEntityAbstractBN(Int64 model, byte[] entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiIsEntityAbstractBN((Int32)model, entityName);
				return _result;
			}
			else
			{
				return x64_engiIsEntityAbstractBN(model, entityName);
			}
		}

		/// <summary>
		///		engiGetEnumerationValue                                 (https://rdf.bg/ifcdoc/CS64/engiGetEnumerationValue.html)
		///
		///	Allows to retrieve enumeration values of an attribute by index.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEnumerationValue")]
		public static extern IntPtr x86_engiGetEnumerationValue(Int32 attribute, Int32 index, Int32 valueType, out IntPtr enumerationValue);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEnumerationValue")]
		public static extern IntPtr x64_engiGetEnumerationValue(Int64 attribute, Int64 index, Int64 valueType, out IntPtr enumerationValue);

		public static IntPtr engiGetEnumerationValue(Int64 attribute, Int64 index, Int64 valueType, out IntPtr enumerationValue)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEnumerationValue((Int32)attribute, (Int32)index, (Int32)valueType, out IntPtr _enumerationValue);
				enumerationValue = _enumerationValue;
				return _result;
			}
			else
			{
				return x64_engiGetEnumerationValue(attribute, index, valueType, out enumerationValue);
			}
		}

		/// <summary>
		///		engiGetEntityAttributeByIterator                        (https://rdf.bg/ifcdoc/CS64/engiGetEntityAttributeByIterator.html)
		///
		///	Iterates attribute definition of the entity.
		///	Includes explicit, inverse and derived attributes defined by this or parent entities.
		///	If a explicit attribute is also known as derived it's reported ones as explicit.
		///	Returns first attribute if prev is NULL.
		///	Returns NULL when prev is the last attribute.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityAttributeByIterator")]
		public static extern Int32 x86_engiGetEntityAttributeByIterator(Int32 entity, Int32 prev);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityAttributeByIterator")]
		public static extern Int64 x64_engiGetEntityAttributeByIterator(Int64 entity, Int64 prev);

		public static Int64 engiGetEntityAttributeByIterator(Int64 entity, Int64 prev)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityAttributeByIterator((Int32)entity, (Int32)prev);
				return _result;
			}
			else
			{
				return x64_engiGetEntityAttributeByIterator(entity, prev);
			}
		}

		/// <summary>
		///		engiGetEntityAttributeByIndex                           (https://rdf.bg/ifcdoc/CS64/engiGetEntityAttributeByIndex.html)
		///
		///	Return attribute definition from attribute index.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityAttributeByIndex")]
		public static extern Int32 x86_engiGetEntityAttributeByIndex(Int32 entity, Int32 index, bool countedWithParents, bool countedWithInverse);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityAttributeByIndex")]
		public static extern Int64 x64_engiGetEntityAttributeByIndex(Int64 entity, Int64 index, bool countedWithParents, bool countedWithInverse);

		public static Int64 engiGetEntityAttributeByIndex(Int64 entity, Int64 index, bool countedWithParents, bool countedWithInverse)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityAttributeByIndex((Int32)entity, (Int32)index, countedWithParents, countedWithInverse);
				return _result;
			}
			else
			{
				return x64_engiGetEntityAttributeByIndex(entity, index, countedWithParents, countedWithInverse);
			}
		}

		/// <summary>
		///		engiGetAggregationDefinition                            (https://rdf.bg/ifcdoc/CS64/engiGetAggregationDefinition.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggregationDefinition")]
		public static extern void x86_engiGetAggregationDefinition(Int32 aggregationDefinition, out enum_express_aggr aggregationType, out Int32 cardinalityMin, out Int32 cardinalityMax, out bool optional, out bool unique, out Int32 nextAggregationLevel);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggregationDefinition")]
		public static extern void x64_engiGetAggregationDefinition(Int64 aggregationDefinition, out enum_express_aggr aggregationType, out Int64 cardinalityMin, out Int64 cardinalityMax, out bool optional, out bool unique, out Int64 nextAggregationLevel);

		public static void engiGetAggregationDefinition(Int64 aggregationDefinition, out enum_express_aggr aggregationType, out Int64 cardinalityMin, out Int64 cardinalityMax, out bool optional, out bool unique, out Int64 nextAggregationLevel)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiGetAggregationDefinition((Int32)aggregationDefinition, out enum_express_aggr _aggregationType, out Int32 _cardinalityMin, out Int32 _cardinalityMax, out bool _optional, out bool _unique, out Int32 _nextAggregationLevel);
				aggregationType = _aggregationType;
				cardinalityMin = _cardinalityMin;
				cardinalityMax = _cardinalityMax;
				optional = _optional;
				unique = _unique;
				nextAggregationLevel = _nextAggregationLevel;
			}
			else
			{
				x64_engiGetAggregationDefinition(aggregationDefinition, out aggregationType, out cardinalityMin, out cardinalityMax, out optional, out unique, out nextAggregationLevel);
			}
		}

		/// <summary>
		///		engiGetEntityUniqueRuleByIterator                       (https://rdf.bg/ifcdoc/CS64/engiGetEntityUniqueRuleByIterator.html)
		///
		///	Iterates unique rules of the entity.
		///	Includes this but not parent entities.
		///	Returns first rule if prev is NULL.
		///	Returns NULL when prev is the last rule.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityUniqueRuleByIterator")]
		public static extern Int32 x86_engiGetEntityUniqueRuleByIterator(Int32 entity, Int32 prev, out IntPtr label);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityUniqueRuleByIterator")]
		public static extern Int64 x64_engiGetEntityUniqueRuleByIterator(Int64 entity, Int64 prev, out IntPtr label);

		public static Int64 engiGetEntityUniqueRuleByIterator(Int64 entity, Int64 prev, out IntPtr label)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityUniqueRuleByIterator((Int32)entity, (Int32)prev, out IntPtr _label);
				label = _label;
				return _result;
			}
			else
			{
				return x64_engiGetEntityUniqueRuleByIterator(entity, prev, out label);
			}
		}

		/// <summary>
		///		engiGetEntityUniqueRuleAttributeByIterator              (https://rdf.bg/ifcdoc/CS64/engiGetEntityUniqueRuleAttributeByIterator.html)
		///
		///	Iterates attributes of unique rule.
		///	Returns first attribute name if prev is NULL.
		///	Returns NULL when prev is the name of the last attribute.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityUniqueRuleAttributeByIterator")]
		public static extern IntPtr x86_engiGetEntityUniqueRuleAttributeByIterator(Int32 rule, string prev, out IntPtr domain);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityUniqueRuleAttributeByIterator")]
		public static extern IntPtr x64_engiGetEntityUniqueRuleAttributeByIterator(Int64 rule, string prev, out IntPtr domain);

		public static IntPtr engiGetEntityUniqueRuleAttributeByIterator(Int64 rule, string prev, out IntPtr domain)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityUniqueRuleAttributeByIterator((Int32)rule, prev, out IntPtr _domain);
				domain = _domain;
				return _result;
			}
			else
			{
				return x64_engiGetEntityUniqueRuleAttributeByIterator(rule, prev, out domain);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityUniqueRuleAttributeByIterator")]
		public static extern IntPtr x86_engiGetEntityUniqueRuleAttributeByIterator(Int32 rule, byte[] prev, out IntPtr domain);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityUniqueRuleAttributeByIterator")]
		public static extern IntPtr x64_engiGetEntityUniqueRuleAttributeByIterator(Int64 rule, byte[] prev, out IntPtr domain);

		public static IntPtr engiGetEntityUniqueRuleAttributeByIterator(Int64 rule, byte[] prev, out IntPtr domain)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityUniqueRuleAttributeByIterator((Int32)rule, prev, out IntPtr _domain);
				domain = _domain;
				return _result;
			}
			else
			{
				return x64_engiGetEntityUniqueRuleAttributeByIterator(rule, prev, out domain);
			}
		}

		/// <summary>
		///		engiGetEntityWhereRuleByIterator                        (https://rdf.bg/ifcdoc/CS64/engiGetEntityWhereRuleByIterator.html)
		///
		///	Iterates where rules of the entity or defined type.
		///	Declaration can be ENTITY or DEFINED_TYPE.
		///	Includes this but not parent entities or types.
		///	Returns first rule if prev is NULL.
		///	Returns NULL when prev is the last rule.
		///	Use engiGetScriptText to get further information.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityWhereRuleByIterator")]
		public static extern Int32 x86_engiGetEntityWhereRuleByIterator(Int32 declaration, Int32 prev, out IntPtr label);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityWhereRuleByIterator")]
		public static extern Int64 x64_engiGetEntityWhereRuleByIterator(Int64 declaration, Int64 prev, out IntPtr label);

		public static Int64 engiGetEntityWhereRuleByIterator(Int64 declaration, Int64 prev, out IntPtr label)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityWhereRuleByIterator((Int32)declaration, (Int32)prev, out IntPtr _label);
				label = _label;
				return _result;
			}
			else
			{
				return x64_engiGetEntityWhereRuleByIterator(declaration, prev, out label);
			}
		}

        //
        //  Instance Reading API Calls
        //

		/// <summary>
		///		sdaiGetADBType                                          (https://rdf.bg/ifcdoc/CS64/sdaiGetADBType.html)
		///
		///	This call can be used to get the used type within this ADB type.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetADBType")]
		public static extern Int32 x86_sdaiGetADBType(Int32 ADB);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetADBType")]
		public static extern Int64 x64_sdaiGetADBType(Int64 ADB);

		public static Int64 sdaiGetADBType(Int64 ADB)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetADBType((Int32)ADB);
				return _result;
			}
			else
			{
				return x64_sdaiGetADBType(ADB);
			}
		}

		/// <summary>
		///		sdaiGetADBTypePath                                      (https://rdf.bg/ifcdoc/CS64/sdaiGetADBTypePath.html)
		///
		///	This call can be used to get the path of an ADB type.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetADBTypePath")]
		public static extern IntPtr x86_sdaiGetADBTypePath(Int32 ADB, Int32 typeNameNumber);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetADBTypePath")]
		public static extern IntPtr x64_sdaiGetADBTypePath(Int64 ADB, Int64 typeNameNumber);

		public static IntPtr sdaiGetADBTypePath(Int64 ADB, Int64 typeNameNumber)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetADBTypePath((Int32)ADB, (Int32)typeNameNumber);
				return _result;
			}
			else
			{
				return x64_sdaiGetADBTypePath(ADB, typeNameNumber);
			}
		}

		/// <summary>
		///		sdaiGetADBValue                                         (https://rdf.bg/ifcdoc/CS64/sdaiGetADBValue.html)
		///
		///	valueType argument to specify what type of data caller wants to get and
		///	value argument where the caller should provide a buffer, and the function will write the result to.
		///
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiGetADBValue, and it works similarly for all get-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///	The Table 2 shows what valueType can be fulfilled depending on actual model data.
		///	If a get-function cannot get a value it will return 0, it may happen when model item is unset ($) or incompatible with requested valueType.
		///	To separate these cases you can use engiGetInstanceAttrType(BN), sdaiGetADBType and engiGetAggrType.
		///	On success get-function will return non-zero. More precisely, according to ISO 10303-24-2001 on success they return content of
		///	value argument (*value) for sdaiADB, sdaiAGGR, or sdaiINSTANCE or value argument itself for other types (it has no useful meaning for C#).
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiGetADBValue but valid for all get-functions)
		///
		///	valueType				C/C++												C#
		///
		///	sdaiINTEGER				int_t val;											int_t val;
		///							sdaiGetADBValue (ADB, sdaiINTEGER, &val);			ifcengine.sdaiGetADBValue (ADB, ifcengine.sdaiINTEGER, out val);
		///
		///	sdaiREAL or sdaiNUMBER	double val;											double val;
		///							sdaiGetADBValue (ADB, sdaiREAL, &val);				ifcengine.sdaiGetADBValue (ADB, ifcengine.sdaiREAL, out val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val;									bool val;
		///							sdaiGetADBValue (ADB, sdaiBOOLEAN, &val);			ifcengine.sdaiGetADBValue (ADB, ifcengine.sdaiBOOLEAN, out val);
		///
		///	sdaiLOGICAL				const TCHAR* val;									string val;
		///							sdaiGetADBValue (ADB, sdaiLOGICAL, &val);			ifcengine.sdaiGetADBValue (ADB, ifcengine.sdaiLOGICAL, out val);
		///
		///	sdaiENUM				const TCHAR* val;									string val;
		///							sdaiGetADBValue (ADB, sdaiENUM, &val);				ifcengine.sdaiGetADBValue (ADB, ifcengine.sdaiENUM, out val);
		///
		///	sdaiBINARY				const TCHAR* val;									string val;
		///							sdaiGetADBValue (ADB, sdaiBINARY, &val);			ifcengine.sdaiGetADBValue (ADB, ifcengine.sdaiBINARY, out val);
		///
		///	sdaiSTRING				const char* val;									string val;
		///							sdaiGetADBValue (ADB, sdaiSTRING, &val);			ifcengine.sdaiGetADBValue (ADB, ifcengine.sdaiSTRING, out val);
		///
		///	sdaiUNICODE				const wchar_t* val;									string val;
		///							sdaiGetADBValue (ADB, sdaiUNICODE, &val);			ifcengine.sdaiGetADBValue (ADB, ifcengine.sdaiUNICODE, out val);
		///
		///	sdaiEXPRESSSTRING		const char* val;									string val;
		///							sdaiGetADBValue (ADB, sdaiEXPRESSSTRING, &val);		ifcengine.sdaiGetADBValue (ADB, ifcengine.sdaiEXPRESSSTRING, out val);
		///
		///	sdaiINSTANCE			SdaiInstance val;									int_t val;
		///							sdaiGetADBValue (ADB, sdaiINSTANCE, &val);			ifcengine.sdaiGetADBValue (ADB, ifcengine.sdaiINSTANCE, out val);
		///
		///	sdaiAGGR				SdaiAggr aggr;										int_t aggr;
		///							sdaiGetADBValue (ADB, sdaiAGGR, &aggr);				ifcengine.sdaiGetADBValue (ADB, ifcengine.sdaiAGGR, out aggr);
		///
		///	sdaiADB					SdaiADB adb = sdaiCreateEmptyADB();					int_t adb = 0;	//	it is important to initialize
		///							sdaiGetADBValue (ADB, sdaiADB, adb);				ifcengine.sdaiGetADBValue (ADB, ifcengine.sdaiADB, out adb);		
		///							sdaiDeleteADB (adb);
		///
		///							SdaiADB adb = nullptr;	//	it is important to initialize
		///							sdaiGetADBValue (ADB, sdaiADB, &adb);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///	(Non-standard extension) sdiADB in C++ has an option to work without sdaiCreateEmptyADB and sdaiDeleteADB as shown in the table.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			Yes *		 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			Yes			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			Yes			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .			 .			 .
		///	sdaiUNICODE			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		///	Note: sdaiGetAttr, stdaiGetAttrBN, engiGetElement will success with any model data, except non-set($)
		///		  (Non-standard extensions) sdaiGetADBValue: sdaiADB is allowed and will success when sdaiGetADBTypePath is not NULL, returning ABD value has type path element removed.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetADBValue")]
		public static extern Int32 x86_sdaiGetADBValue(Int32 ADB, Int32 valueType, out bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetADBValue")]
		public static extern Int64 x64_sdaiGetADBValue(Int64 ADB, Int64 valueType, out bool value);

		public static Int64 sdaiGetADBValue(Int64 ADB, Int64 valueType, out bool value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetADBValue((Int32)ADB, (Int32)valueType, out bool _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetADBValue(ADB, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetADBValue")]
		public static extern Int32 x86_sdaiGetADBValue(Int32 ADB, Int32 valueType, out Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetADBValue")]
		public static extern Int64 x64_sdaiGetADBValue(Int64 ADB, Int64 valueType, out Int64 value);

		public static Int64 sdaiGetADBValue(Int64 ADB, Int64 valueType, out Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetADBValue((Int32)ADB, (Int32)valueType, out Int32 _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetADBValue(ADB, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetADBValue")]
		public static extern Int32 x86_sdaiGetADBValue(Int32 ADB, Int32 valueType, out double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetADBValue")]
		public static extern Int64 x64_sdaiGetADBValue(Int64 ADB, Int64 valueType, out double value);

		public static Int64 sdaiGetADBValue(Int64 ADB, Int64 valueType, out double value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetADBValue((Int32)ADB, (Int32)valueType, out double _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetADBValue(ADB, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetADBValue")]
		public static extern Int32 x86_sdaiGetADBValue(Int32 ADB, Int32 valueType, out IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetADBValue")]
		public static extern Int64 x64_sdaiGetADBValue(Int64 ADB, Int64 valueType, out IntPtr value);

		public static Int64 sdaiGetADBValue(Int64 ADB, Int64 valueType, out IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetADBValue((Int32)ADB, (Int32)valueType, out IntPtr _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetADBValue(ADB, valueType, out value);
			}
		}

		/// <summary>
		///		sdaiPutADBValue                                         (https://rdf.bg/ifcdoc/CS64/sdaiPutADBValue.html)
		///
		///	valueType argument to specify what type of data caller wants to put
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiPutADBValue, and it works similarly for all put-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiPutADBValue but valid for all put-functions)
		///
		///	valueType				C/C++														C#
		///
		///	sdaiINTEGER				int_t val = 123;											int_t val = 123;
		///							sdaiPutADBValue (ADB, sdaiINTEGER, &val);					ifcengine.sdaiPutADBValue (ADB, ifcengine.sdaiINTEGER, ref val);
		///
		///	sdaiREAL or sdaiNUMBER	double val = 123.456;										double val = 123.456;
		///							sdaiPutADBValue (ADB, sdaiREAL, &val);						ifcengine.sdaiPutADBValue (ADB, ifcengine.sdaiREAL, ref val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val = sdaiTRUE;									bool val = true;
		///							sdaiPutADBValue (ADB, sdaiBOOLEAN, &val);					ifcengine.sdaiPutADBValue (ADB, ifcengine.sdaiBOOLEAN, ref val);
		///
		///	sdaiLOGICAL				const TCHAR* val = "U";										string val = "U";
		///							sdaiPutADBValue (ADB, sdaiLOGICAL, val);					ifcengine.sdaiPutADBValue (ADB, ifcengine.sdaiLOGICAL, val);
		///
		///	sdaiENUM				const TCHAR* val = "NOTDEFINED";							string val = "NOTDEFINED";
		///							sdaiPutADBValue (ADB, sdaiENUM, val);						ifcengine.sdaiPutADBValue (ADB, ifcengine.sdaiENUM, val);
		///
		///	sdaiBINARY				const TCHAR* val = "0123456ABC";							string val = "0123456ABC";
		///							sdaiPutADBValue (ADB, sdaiBINARY, val);						ifcengine.sdaiPutADBValue (ADB, ifcengine.sdaiBINARY, val);
		///
		///	sdaiSTRING				const char* val = "My Simple String";						string val = "My Simple String";
		///							sdaiPutADBValue (ADB, sdaiSTRING, val);						ifcengine.sdaiPutADBValue (ADB, ifcengine.sdaiSTRING, val);
		///
		///	sdaiUNICODE				const wchar_t* val = L"Any Unicode String";					string val = "Any Unicode String";
		///							sdaiPutADBValue (ADB, sdaiUNICODE, val);					ifcengine.sdaiPutADBValue (ADB, ifcengine.sdaiUNICODE, val);
		///
		///	sdaiEXPRESSSTRING		const char* val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";	string val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";
		///							sdaiPutADBValue (ADB, sdaiEXPRESSSTRING, val);				ifcengine.sdaiPutADBValue (ADB, ifcengine.sdaiEXPRESSSTRING, val);
		///
		///	sdaiINSTANCE			SdaiInstance val = sdaiCreateInstanceBN (model, "IFCSITE");	int_t val = ifcengine.sdaiCreateInstanceBN (model, "IFCSITE");
		///							sdaiPutADBValue (ADB, sdaiINSTANCE, val);					ifcengine.sdaiPutADBValue (ADB, ifcengine.sdaiINSTANCE, val);
		///
		///	sdaiAGGR				SdaiAggr val = sdaiCreateAggr (inst, 0);					int_t val = sdaiCreateAggr (inst, 0);
		///							sdaiPutAttr (val, sdaiINSTANCE, inst);						ifcengine.sdaiPutAttr (val, ifcengine.sdaiINSTANCE, inst);
		///							sdaiPutADBValue (ADB, sdaiAGGR, val);						ifcengine.sdaiPutADBValue (ADB, ifcengine.sdaiAGGR, val);
		///
		///	sdaiADB					int_t integerValue = 123;									int_t integerValue = 123;	
		///							SdaiADB val = sdaiCreateADB (sdaiINTEGER, &integerValue);	int_t val = ifcengine.sdaiCreateADB (ifcengine.sdaiINTEGER, ref integerValue);
		///							sdaiPutADBTypePath (val, 1, "IFCINTEGER");					ifcengine.sdaiPutADBTypePath (val, 1, "IFCINTEGER");
		///							sdaiPutADBValue (ADB, sdaiADB, val);						ifcengine.sdaiPutADBValue (ADB, ifcengine.sdaiADB, val);	
		///							sdaiDeleteADB (val);										ifcengine.sdaiDeleteADB (val);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///	(Non-standard extension) sdiADB in C++ has an option to work without sdaiCreateEmptyADB and sdaiDeleteADB as shown in the table.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			 .			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			 .			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			 . 			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiUNICODE			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBValue")]
		public static extern void x86_sdaiPutADBValue(Int32 ADB, Int32 valueType, ref bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBValue")]
		public static extern void x64_sdaiPutADBValue(Int64 ADB, Int64 valueType, ref bool value);

		public static void sdaiPutADBValue(Int64 ADB, Int64 valueType, ref bool value)
		{
			if (IntPtr.Size == 4)
			{
				bool _value = (bool)value;
				x86_sdaiPutADBValue((Int32)ADB, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutADBValue(ADB, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBValue")]
		public static extern void x86_sdaiPutADBValue(Int32 ADB, Int32 valueType, ref Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBValue")]
		public static extern void x64_sdaiPutADBValue(Int64 ADB, Int64 valueType, ref Int64 value);

		public static void sdaiPutADBValue(Int64 ADB, Int64 valueType, ref Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				Int32 _value = (Int32)value;
				x86_sdaiPutADBValue((Int32)ADB, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutADBValue(ADB, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBValue")]
		public static extern void x86_sdaiPutADBValue(Int32 ADB, Int32 valueType, Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBValue")]
		public static extern void x64_sdaiPutADBValue(Int64 ADB, Int64 valueType, Int64 value);

		public static void sdaiPutADBValue(Int64 ADB, Int64 valueType, Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutADBValue((Int32)ADB, (Int32)valueType, (Int32)value);
			}
			else
			{
				x64_sdaiPutADBValue(ADB, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBValue")]
		public static extern void x86_sdaiPutADBValue(Int32 ADB, Int32 valueType, ref double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBValue")]
		public static extern void x64_sdaiPutADBValue(Int64 ADB, Int64 valueType, ref double value);

		public static void sdaiPutADBValue(Int64 ADB, Int64 valueType, ref double value)
		{
			if (IntPtr.Size == 4)
			{
				double _value = (double)value;
				x86_sdaiPutADBValue((Int32)ADB, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutADBValue(ADB, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBValue")]
		public static extern void x86_sdaiPutADBValue(Int32 ADB, Int32 valueType, ref IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBValue")]
		public static extern void x64_sdaiPutADBValue(Int64 ADB, Int64 valueType, ref IntPtr value);

		public static void sdaiPutADBValue(Int64 ADB, Int64 valueType, ref IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				IntPtr _value = (IntPtr)value;
				x86_sdaiPutADBValue((Int32)ADB, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutADBValue(ADB, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBValue")]
		public static extern void x86_sdaiPutADBValue(Int32 ADB, Int32 valueType, byte[] value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBValue")]
		public static extern void x64_sdaiPutADBValue(Int64 ADB, Int64 valueType, byte[] value);

		public static void sdaiPutADBValue(Int64 ADB, Int64 valueType, byte[] value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutADBValue((Int32)ADB, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiPutADBValue(ADB, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBValue")]
		public static extern void x86_sdaiPutADBValue(Int32 ADB, Int32 valueType, string value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBValue")]
		public static extern void x64_sdaiPutADBValue(Int64 ADB, Int64 valueType, string value);

		public static void sdaiPutADBValue(Int64 ADB, Int64 valueType, string value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutADBValue((Int32)ADB, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiPutADBValue(ADB, valueType, value);
			}
		}

		/// <summary>
		///		sdaiCreateEmptyADB                                      (https://rdf.bg/ifcdoc/CS64/sdaiCreateEmptyADB.html)
		///
		///	Creates an empty ADB (Attribute Data Block).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateEmptyADB")]
		public static extern Int32 x86_sdaiCreateEmptyADB();

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateEmptyADB")]
		public static extern Int64 x64_sdaiCreateEmptyADB();

		public static Int64 sdaiCreateEmptyADB()
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateEmptyADB();
				return _result;
			}
			else
			{
				return x64_sdaiCreateEmptyADB();
			}
		}

		/// <summary>
		///		sdaiDeleteADB                                           (https://rdf.bg/ifcdoc/CS64/sdaiDeleteADB.html)
		///
		///	Deletes an ADB (Attribute Data Block).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiDeleteADB")]
		public static extern void x86_sdaiDeleteADB(Int32 ADB);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiDeleteADB")]
		public static extern void x64_sdaiDeleteADB(Int64 ADB);

		public static void sdaiDeleteADB(Int64 ADB)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiDeleteADB((Int32)ADB);
			}
			else
			{
				x64_sdaiDeleteADB(ADB);
			}
		}

		/// <summary>
		///		sdaiGetAggrByIndex                                      (https://rdf.bg/ifcdoc/CS64/sdaiGetAggrByIndex.html)
		///
		///	valueType argument to specify what type of data caller wants to get and
		///	value argument where the caller should provide a buffer, and the function will write the result to.
		///
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiGetAggrByIndex, and it works similarly for all get-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///	The Table 2 shows what valueType can be fulfilled depending on actual model data.
		///	If a get-function cannot get a value it will return 0, it may happen when model item is unset ($) or incompatible with requested valueType.
		///	To separate these cases you can use engiGetInstanceAttrType(BN), sdaiGetADBType and engiGetAggrType.
		///	On success get-function will return non-zero. More precisely, according to ISO 10303-24-2001 on success they return content of
		///	value argument (*value) for sdaiADB, sdaiAGGR, or sdaiINSTANCE or value argument itself for other types (it has no useful meaning for C#).
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiGetAggrByIndex but valid for all get-functions)
		///
		///	valueType				C/C++																C#
		///
		///	sdaiINTEGER				int_t val;															int_t val;
		///							sdaiGetAggrByIndex (aggregate, index, sdaiINTEGER, &val);			ifcengine.sdaiGetAggrByIndex (aggregate, index, ifcengine.sdaiINTEGER, out val);
		///
		///	sdaiREAL or sdaiNUMBER	double val;															double val;
		///							sdaiGetAggrByIndex (aggregate, index, sdaiREAL, &val);				ifcengine.sdaiGetAggrByIndex (aggregate, index, ifcengine.sdaiREAL, out val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val;													bool val;
		///							sdaiGetAggrByIndex (aggregate, index, sdaiBOOLEAN, &val);			ifcengine.sdaiGetAggrByIndex (aggregate, index, ifcengine.sdaiBOOLEAN, out val);
		///
		///	sdaiLOGICAL				const TCHAR* val;													string val;
		///							sdaiGetAggrByIndex (aggregate, index, sdaiLOGICAL, &val);			ifcengine.sdaiGetAggrByIndex (aggregate, index, ifcengine.sdaiLOGICAL, out val);
		///
		///	sdaiENUM				const TCHAR* val;													string val;
		///							sdaiGetAggrByIndex (aggregate, index, sdaiENUM, &val);				ifcengine.sdaiGetAggrByIndex (aggregate, index, ifcengine.sdaiENUM, out val);
		///
		///	sdaiBINARY				const TCHAR* val;													string val;
		///							sdaiGetAggrByIndex (aggregate, index, sdaiBINARY, &val);			ifcengine.sdaiGetAggrByIndex (aggregate, index, ifcengine.sdaiBINARY, out val);
		///
		///	sdaiSTRING				const char* val;													string val;
		///							sdaiGetAggrByIndex (aggregate, index, sdaiSTRING, &val);			ifcengine.sdaiGetAggrByIndex (aggregate, index, ifcengine.sdaiSTRING, out val);
		///
		///	sdaiUNICODE				const wchar_t* val;													string val;
		///							sdaiGetAggrByIndex (aggregate, index, sdaiUNICODE, &val);			ifcengine.sdaiGetAggrByIndex (aggregate, index, ifcengine.sdaiUNICODE, out val);
		///
		///	sdaiEXPRESSSTRING		const char* val;													string val;
		///							sdaiGetAggrByIndex (aggregate, index, sdaiEXPRESSSTRING, &val);		ifcengine.sdaiGetAggrByIndex (aggregate, index, ifcengine.sdaiEXPRESSSTRING, out val);
		///
		///	sdaiINSTANCE			SdaiInstance val;													int_t val;
		///							sdaiGetAggrByIndex (aggregate, index, sdaiINSTANCE, &val);			ifcengine.sdaiGetAggrByIndex (aggregate, index, ifcengine.sdaiINSTANCE, out val);
		///
		///	sdaiAGGR				SdaiAggr aggr;														int_t aggr;
		///							sdaiGetAggrByIndex (aggregate, index, sdaiAGGR, &aggr);				ifcengine.sdaiGetAggrByIndex (aggregate, index, ifcengine.sdaiAGGR, out aggr);
		///
		///	sdaiADB					SdaiADB adb = sdaiCreateEmptyADB();									int_t adb = 0;	//	it is important to initialize
		///							sdaiGetAggrByIndex (aggregate, index, sdaiADB, adb);				ifcengine.sdaiGetAggrByIndex (aggregate, index, ifcengine.sdaiADB, out adb);		
		///							sdaiDeleteADB (adb);
		///
		///							SdaiADB adb = nullptr;	//	it is important to initialize
		///							sdaiGetAggrByIndex (aggregate, index, sdaiADB, &adb);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///	(Non-standard extension) sdiADB in C++ has an option to work without sdaiCreateEmptyADB and sdaiDeleteADB as shown in the table.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			Yes *		 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			Yes			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			Yes			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .			 .			 .
		///	sdaiUNICODE			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		///	Note: sdaiGetAttr, stdaiGetAttrBN, engiGetElement will success with any model data, except non-set($)
		///		  (Non-standard extensions) sdaiGetADBValue: sdaiADB is allowed and will success when sdaiGetADBTypePath is not NULL, returning ABD value has type path element removed.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrByIndex")]
		public static extern Int32 x86_sdaiGetAggrByIndex(Int32 aggregate, Int32 index, Int32 valueType, out bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrByIndex")]
		public static extern Int64 x64_sdaiGetAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, out bool value);

		public static Int64 sdaiGetAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, out bool value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAggrByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, out bool _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAggrByIndex(aggregate, index, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrByIndex")]
		public static extern Int32 x86_sdaiGetAggrByIndex(Int32 aggregate, Int32 index, Int32 valueType, out Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrByIndex")]
		public static extern Int64 x64_sdaiGetAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, out Int64 value);

		public static Int64 sdaiGetAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, out Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAggrByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, out Int32 _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAggrByIndex(aggregate, index, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrByIndex")]
		public static extern Int32 x86_sdaiGetAggrByIndex(Int32 aggregate, Int32 index, Int32 valueType, out double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrByIndex")]
		public static extern Int64 x64_sdaiGetAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, out double value);

		public static Int64 sdaiGetAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, out double value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAggrByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, out double _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAggrByIndex(aggregate, index, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrByIndex")]
		public static extern Int32 x86_sdaiGetAggrByIndex(Int32 aggregate, Int32 index, Int32 valueType, out IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrByIndex")]
		public static extern Int64 x64_sdaiGetAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, out IntPtr value);

		public static Int64 sdaiGetAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, out IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAggrByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, out IntPtr _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAggrByIndex(aggregate, index, valueType, out value);
			}
		}

		/// <summary>
		///		sdaiPutAggrByIndex                                      (https://rdf.bg/ifcdoc/CS64/sdaiPutAggrByIndex.html)
		///
		///	valueType argument to specify what type of data caller wants to put
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiPutAggrByIndex, and it works similarly for all put-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiPutAggrByIndex but valid for all put-functions)
		///
		///	valueType				C/C++															C#
		///
		///	sdaiINTEGER				int_t val = 123;												int_t val = 123;
		///							sdaiPutAggrByIndex (aggregate, index, sdaiINTEGER, &val);		ifcengine.sdaiPutAggrByIndex (aggregate, index, ifcengine.sdaiINTEGER, ref val);
		///
		///	sdaiREAL or sdaiNUMBER	double val = 123.456;											double val = 123.456;
		///							sdaiPutAggrByIndex (aggregate, index, sdaiREAL, &val);			ifcengine.sdaiPutAggrByIndex (aggregate, index, ifcengine.sdaiREAL, ref val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val = sdaiTRUE;										bool val = true;
		///							sdaiPutAggrByIndex (aggregate, index, sdaiBOOLEAN, &val);		ifcengine.sdaiPutAggrByIndex (aggregate, index, ifcengine.sdaiBOOLEAN, ref val);
		///
		///	sdaiLOGICAL				const TCHAR* val = "U";											string val = "U";
		///							sdaiPutAggrByIndex (aggregate, index, sdaiLOGICAL, val);		ifcengine.sdaiPutAggrByIndex (aggregate, index, ifcengine.sdaiLOGICAL, val);
		///
		///	sdaiENUM				const TCHAR* val = "NOTDEFINED";								string val = "NOTDEFINED";
		///							sdaiPutAggrByIndex (aggregate, index, sdaiENUM, val);			ifcengine.sdaiPutAggrByIndex (aggregate, index, ifcengine.sdaiENUM, val);
		///
		///	sdaiBINARY				const TCHAR* val = "0123456ABC";								string val = "0123456ABC";
		///							sdaiPutAggrByIndex (aggregate, index, sdaiBINARY, val);			ifcengine.sdaiPutAggrByIndex (aggregate, index, ifcengine.sdaiBINARY, val);
		///
		///	sdaiSTRING				const char* val = "My Simple String";							string val = "My Simple String";
		///							sdaiPutAggrByIndex (aggregate, index, sdaiSTRING, val);			ifcengine.sdaiPutAggrByIndex (aggregate, index, ifcengine.sdaiSTRING, val);
		///
		///	sdaiUNICODE				const wchar_t* val = L"Any Unicode String";						string val = "Any Unicode String";
		///							sdaiPutAggrByIndex (aggregate, index, sdaiUNICODE, val);		ifcengine.sdaiPutAggrByIndex (aggregate, index, ifcengine.sdaiUNICODE, val);
		///
		///	sdaiEXPRESSSTRING		const char* val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";		string val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";
		///							sdaiPutAggrByIndex (aggregate, index, sdaiEXPRESSSTRING, val);	ifcengine.sdaiPutAggrByIndex (aggregate, index, ifcengine.sdaiEXPRESSSTRING, val);
		///
		///	sdaiINSTANCE			SdaiInstance val = sdaiCreateInstanceBN (model, "IFCSITE");		int_t val = ifcengine.sdaiCreateInstanceBN (model, "IFCSITE");
		///							sdaiPutAggrByIndex (aggregate, index, sdaiINSTANCE, val);		ifcengine.sdaiPutAggrByIndex (aggregate, index, ifcengine.sdaiINSTANCE, val);
		///
		///	sdaiAGGR				SdaiAggr val = sdaiCreateAggr (inst, 0);						int_t val = sdaiCreateAggr (inst, 0);
		///							sdaiPutAttr (val, sdaiINSTANCE, inst);							ifcengine.sdaiPutAttr (val, ifcengine.sdaiINSTANCE, inst);
		///							sdaiPutAggrByIndex (aggregate, index, sdaiAGGR, val);			ifcengine.sdaiPutAggrByIndex (aggregate, index, ifcengine.sdaiAGGR, val);
		///
		///	sdaiADB					int_t integerValue = 123;										int_t integerValue = 123;	
		///							SdaiADB val = sdaiCreateADB (sdaiINTEGER, &integerValue);		int_t val = ifcengine.sdaiCreateADB (ifcengine.sdaiINTEGER, ref integerValue);
		///							sdaiPutADBTypePath (val, 1, "IFCINTEGER");						ifcengine.sdaiPutADBTypePath (val, 1, "IFCINTEGER");
		///							sdaiPutAggrByIndex (aggregate, index, sdaiADB, val);			ifcengine.sdaiPutAggrByIndex (aggregate, index, ifcengine.sdaiADB, val);	
		///							sdaiDeleteADB (val);											ifcengine.sdaiDeleteADB (val);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///	(Non-standard extension) sdiADB in C++ has an option to work without sdaiCreateEmptyADB and sdaiDeleteADB as shown in the table.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			 .			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			 .			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			 . 			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiUNICODE			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIndex")]
		public static extern void x86_sdaiPutAggrByIndex(Int32 aggregate, Int32 index, Int32 valueType, ref bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIndex")]
		public static extern void x64_sdaiPutAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, ref bool value);

		public static void sdaiPutAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, ref bool value)
		{
			if (IntPtr.Size == 4)
			{
				bool _value = (bool)value;
				x86_sdaiPutAggrByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAggrByIndex(aggregate, index, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIndex")]
		public static extern void x86_sdaiPutAggrByIndex(Int32 aggregate, Int32 index, Int32 valueType, ref Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIndex")]
		public static extern void x64_sdaiPutAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, ref Int64 value);

		public static void sdaiPutAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, ref Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				Int32 _value = (Int32)value;
				x86_sdaiPutAggrByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAggrByIndex(aggregate, index, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIndex")]
		public static extern void x86_sdaiPutAggrByIndex(Int32 aggregate, Int32 index, Int32 valueType, Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIndex")]
		public static extern void x64_sdaiPutAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, Int64 value);

		public static void sdaiPutAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutAggrByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, (Int32)value);
			}
			else
			{
				x64_sdaiPutAggrByIndex(aggregate, index, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIndex")]
		public static extern void x86_sdaiPutAggrByIndex(Int32 aggregate, Int32 index, Int32 valueType, ref double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIndex")]
		public static extern void x64_sdaiPutAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, ref double value);

		public static void sdaiPutAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, ref double value)
		{
			if (IntPtr.Size == 4)
			{
				double _value = (double)value;
				x86_sdaiPutAggrByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAggrByIndex(aggregate, index, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIndex")]
		public static extern void x86_sdaiPutAggrByIndex(Int32 aggregate, Int32 index, Int32 valueType, ref IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIndex")]
		public static extern void x64_sdaiPutAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, ref IntPtr value);

		public static void sdaiPutAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, ref IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				IntPtr _value = (IntPtr)value;
				x86_sdaiPutAggrByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAggrByIndex(aggregate, index, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIndex")]
		public static extern void x86_sdaiPutAggrByIndex(Int32 aggregate, Int32 index, Int32 valueType, byte[] value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIndex")]
		public static extern void x64_sdaiPutAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, byte[] value);

		public static void sdaiPutAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, byte[] value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutAggrByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiPutAggrByIndex(aggregate, index, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIndex")]
		public static extern void x86_sdaiPutAggrByIndex(Int32 aggregate, Int32 index, Int32 valueType, string value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIndex")]
		public static extern void x64_sdaiPutAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, string value);

		public static void sdaiPutAggrByIndex(Int64 aggregate, Int64 index, Int64 valueType, string value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutAggrByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiPutAggrByIndex(aggregate, index, valueType, value);
			}
		}

		/// <summary>
		///		engiGetAggrType                                         (https://rdf.bg/ifcdoc/CS64/engiGetAggrType.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrType")]
		public static extern void x86_engiGetAggrType(Int32 aggregate, out Int32 aggregateType);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrType")]
		public static extern void x64_engiGetAggrType(Int64 aggregate, out Int64 aggregateType);

		public static void engiGetAggrType(Int64 aggregate, out Int64 aggregateType)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiGetAggrType((Int32)aggregate, out Int32 _aggregateType);
				aggregateType = _aggregateType;
			}
			else
			{
				x64_engiGetAggrType(aggregate, out aggregateType);
			}
		}

		/// <summary>
		///		engiGetAggrTypex                                        (https://rdf.bg/ifcdoc/CS64/engiGetAggrTypex.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrTypex")]
		public static extern void x86_engiGetAggrTypex(Int32 aggregate, out Int32 aggregateType);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrTypex")]
		public static extern void x64_engiGetAggrTypex(Int64 aggregate, out Int64 aggregateType);

		public static void engiGetAggrTypex(Int64 aggregate, out Int64 aggregateType)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiGetAggrTypex((Int32)aggregate, out Int32 _aggregateType);
				aggregateType = _aggregateType;
			}
			else
			{
				x64_engiGetAggrTypex(aggregate, out aggregateType);
			}
		}

		/// <summary>
		///		sdaiGetAttr                                             (https://rdf.bg/ifcdoc/CS64/sdaiGetAttr.html)
		///
		///	valueType argument to specify what type of data caller wants to get and
		///	value argument where the caller should provide a buffer, and the function will write the result to.
		///
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiGetAttr, and it works similarly for all get-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///	The Table 2 shows what valueType can be fulfilled depending on actual model data.
		///	If a get-function cannot get a value it will return 0, it may happen when model item is unset ($) or incompatible with requested valueType.
		///	To separate these cases you can use engiGetInstanceAttrType(BN), sdaiGetADBType and engiGetAggrType.
		///	On success get-function will return non-zero. More precisely, according to ISO 10303-24-2001 on success they return content of
		///	value argument (*value) for sdaiADB, sdaiAGGR, or sdaiINSTANCE or value argument itself for other types (it has no useful meaning for C#).
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiGetAttr but valid for all get-functions)
		///
		///	valueType				C/C++															C#
		///
		///	sdaiINTEGER				int_t val;														int_t val;
		///							sdaiGetAttr (instance, attribute, sdaiINTEGER, &val);			ifcengine.sdaiGetAttr (instance, attribute, ifcengine.sdaiINTEGER, out val);
		///
		///	sdaiREAL or sdaiNUMBER	double val;														double val;
		///							sdaiGetAttr (instance, attribute, sdaiREAL, &val);				ifcengine.sdaiGetAttr (instance, attribute, ifcengine.sdaiREAL, out val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val;												bool val;
		///							sdaiGetAttr (instance, attribute, sdaiBOOLEAN, &val);			ifcengine.sdaiGetAttr (instance, attribute, ifcengine.sdaiBOOLEAN, out val);
		///
		///	sdaiLOGICAL				const TCHAR* val;												string val;
		///							sdaiGetAttr (instance, attribute, sdaiLOGICAL, &val);			ifcengine.sdaiGetAttr (instance, attribute, ifcengine.sdaiLOGICAL, out val);
		///
		///	sdaiENUM				const TCHAR* val;												string val;
		///							sdaiGetAttr (instance, attribute, sdaiENUM, &val);				ifcengine.sdaiGetAttr (instance, attribute, ifcengine.sdaiENUM, out val);
		///
		///	sdaiBINARY				const TCHAR* val;												string val;
		///							sdaiGetAttr (instance, attribute, sdaiBINARY, &val);			ifcengine.sdaiGetAttr (instance, attribute, ifcengine.sdaiBINARY, out val);
		///
		///	sdaiSTRING				const char* val;												string val;
		///							sdaiGetAttr (instance, attribute, sdaiSTRING, &val);			ifcengine.sdaiGetAttr (instance, attribute, ifcengine.sdaiSTRING, out val);
		///
		///	sdaiUNICODE				const wchar_t* val;												string val;
		///							sdaiGetAttr (instance, attribute, sdaiUNICODE, &val);			ifcengine.sdaiGetAttr (instance, attribute, ifcengine.sdaiUNICODE, out val);
		///
		///	sdaiEXPRESSSTRING		const char* val;												string val;
		///							sdaiGetAttr (instance, attribute, sdaiEXPRESSSTRING, &val);		ifcengine.sdaiGetAttr (instance, attribute, ifcengine.sdaiEXPRESSSTRING, out val);
		///
		///	sdaiINSTANCE			SdaiInstance val;												int_t val;
		///							sdaiGetAttr (instance, attribute, sdaiINSTANCE, &val);			ifcengine.sdaiGetAttr (instance, attribute, ifcengine.sdaiINSTANCE, out val);
		///
		///	sdaiAGGR				SdaiAggr aggr;													int_t aggr;
		///							sdaiGetAttr (instance, attribute, sdaiAGGR, &aggr);				ifcengine.sdaiGetAttr (instance, attribute, ifcengine.sdaiAGGR, out aggr);
		///
		///	sdaiADB					SdaiADB adb = sdaiCreateEmptyADB();								int_t adb = 0;	//	it is important to initialize
		///							sdaiGetAttr (instance, attribute, sdaiADB, adb);				ifcengine.sdaiGetAttr (instance, attribute, ifcengine.sdaiADB, out adb);		
		///							sdaiDeleteADB (adb);
		///
		///							SdaiADB adb = nullptr;	//	it is important to initialize
		///							sdaiGetAttr (instance, attribute, sdaiADB, &adb);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///	(Non-standard extension) sdiADB in C++ has an option to work without sdaiCreateEmptyADB and sdaiDeleteADB as shown in the table.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			Yes *		 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			Yes			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			Yes			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .			 .			 .
		///	sdaiUNICODE			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		///	Note: sdaiGetAttr, stdaiGetAttrBN, engiGetElement will success with any model data, except non-set($)
		///		  (Non-standard extensions) sdaiGetADBValue: sdaiADB is allowed and will success when sdaiGetADBTypePath is not NULL, returning ABD value has type path element removed.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttr")]
		public static extern Int32 x86_sdaiGetAttr(Int32 instance, Int32 attribute, Int32 valueType, out bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttr")]
		public static extern Int64 x64_sdaiGetAttr(Int64 instance, Int64 attribute, Int64 valueType, out bool value);

		public static Int64 sdaiGetAttr(Int64 instance, Int64 attribute, Int64 valueType, out bool value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAttr((Int32)instance, (Int32)attribute, (Int32)valueType, out bool _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAttr(instance, attribute, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttr")]
		public static extern Int32 x86_sdaiGetAttr(Int32 instance, Int32 attribute, Int32 valueType, out Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttr")]
		public static extern Int64 x64_sdaiGetAttr(Int64 instance, Int64 attribute, Int64 valueType, out Int64 value);

		public static Int64 sdaiGetAttr(Int64 instance, Int64 attribute, Int64 valueType, out Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAttr((Int32)instance, (Int32)attribute, (Int32)valueType, out Int32 _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAttr(instance, attribute, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttr")]
		public static extern Int32 x86_sdaiGetAttr(Int32 instance, Int32 attribute, Int32 valueType, out double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttr")]
		public static extern Int64 x64_sdaiGetAttr(Int64 instance, Int64 attribute, Int64 valueType, out double value);

		public static Int64 sdaiGetAttr(Int64 instance, Int64 attribute, Int64 valueType, out double value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAttr((Int32)instance, (Int32)attribute, (Int32)valueType, out double _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAttr(instance, attribute, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttr")]
		public static extern Int32 x86_sdaiGetAttr(Int32 instance, Int32 attribute, Int32 valueType, out IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttr")]
		public static extern Int64 x64_sdaiGetAttr(Int64 instance, Int64 attribute, Int64 valueType, out IntPtr value);

		public static Int64 sdaiGetAttr(Int64 instance, Int64 attribute, Int64 valueType, out IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAttr((Int32)instance, (Int32)attribute, (Int32)valueType, out IntPtr _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAttr(instance, attribute, valueType, out value);
			}
		}

		/// <summary>
		///		sdaiGetAttrBN                                           (https://rdf.bg/ifcdoc/CS64/sdaiGetAttrBN.html)
		///
		///	valueType argument to specify what type of data caller wants to get and
		///	value argument where the caller should provide a buffer, and the function will write the result to.
		///
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiGetAttrBN, and it works similarly for all get-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///	The Table 2 shows what valueType can be fulfilled depending on actual model data.
		///	If a get-function cannot get a value it will return 0, it may happen when model item is unset ($) or incompatible with requested valueType.
		///	To separate these cases you can use engiGetInstanceAttrType(BN), sdaiGetADBType and engiGetAggrType.
		///	On success get-function will return non-zero. More precisely, according to ISO 10303-24-2001 on success they return content of
		///	value argument (*value) for sdaiADB, sdaiAGGR, or sdaiINSTANCE or value argument itself for other types (it has no useful meaning for C#).
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiGetAttrBN but valid for all get-functions)
		///
		///	valueType				C/C++																C#
		///
		///	sdaiINTEGER				int_t val;															int_t val;
		///							sdaiGetAttrBN (instance, "attrName", sdaiINTEGER, &val);			ifcengine.sdaiGetAttrBN (instance, "attrName", ifcengine.sdaiINTEGER, out val);
		///
		///	sdaiREAL or sdaiNUMBER	double val;															double val;
		///							sdaiGetAttrBN (instance, "attrName", sdaiREAL, &val);				ifcengine.sdaiGetAttrBN (instance, "attrName", ifcengine.sdaiREAL, out val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val;													bool val;
		///							sdaiGetAttrBN (instance, "attrName", sdaiBOOLEAN, &val);			ifcengine.sdaiGetAttrBN (instance, "attrName", ifcengine.sdaiBOOLEAN, out val);
		///
		///	sdaiLOGICAL				const TCHAR* val;													string val;
		///							sdaiGetAttrBN (instance, "attrName", sdaiLOGICAL, &val);			ifcengine.sdaiGetAttrBN (instance, "attrName", ifcengine.sdaiLOGICAL, out val);
		///
		///	sdaiENUM				const TCHAR* val;													string val;
		///							sdaiGetAttrBN (instance, "attrName", sdaiENUM, &val);				ifcengine.sdaiGetAttrBN (instance, "attrName", ifcengine.sdaiENUM, out val);
		///
		///	sdaiBINARY				const TCHAR* val;													string val;
		///							sdaiGetAttrBN (instance, "attrName", sdaiBINARY, &val);				ifcengine.sdaiGetAttrBN (instance, "attrName", ifcengine.sdaiBINARY, out val);
		///
		///	sdaiSTRING				const char* val;													string val;
		///							sdaiGetAttrBN (instance, "attrName", sdaiSTRING, &val);				ifcengine.sdaiGetAttrBN (instance, "attrName", ifcengine.sdaiSTRING, out val);
		///
		///	sdaiUNICODE				const wchar_t* val;													string val;
		///							sdaiGetAttrBN (instance, "attrName", sdaiUNICODE, &val);			ifcengine.sdaiGetAttrBN (instance, "attrName", ifcengine.sdaiUNICODE, out val);
		///
		///	sdaiEXPRESSSTRING		const char* val;													string val;
		///							sdaiGetAttrBN (instance, "attrName", sdaiEXPRESSSTRING, &val);		ifcengine.sdaiGetAttrBN (instance, "attrName", ifcengine.sdaiEXPRESSSTRING, out val);
		///
		///	sdaiINSTANCE			SdaiInstance val;													int_t val;
		///							sdaiGetAttrBN (instance, "attrName", sdaiINSTANCE, &val);			ifcengine.sdaiGetAttrBN (instance, "attrName", ifcengine.sdaiINSTANCE, out val);
		///
		///	sdaiAGGR				SdaiAggr aggr;														int_t aggr;
		///							sdaiGetAttrBN (instance, "attrName", sdaiAGGR, &aggr);				ifcengine.sdaiGetAttrBN (instance, "attrName", ifcengine.sdaiAGGR, out aggr);
		///
		///	sdaiADB					SdaiADB adb = sdaiCreateEmptyADB();									int_t adb = 0;	//	it is important to initialize
		///							sdaiGetAttrBN (instance, "attrName", sdaiADB, adb);					ifcengine.sdaiGetAttrBN (instance, "attrName", ifcengine.sdaiADB, out adb);		
		///							sdaiDeleteADB (adb);
		///
		///							SdaiADB adb = nullptr;	//	it is important to initialize
		///							sdaiGetAttrBN (instance, "attrName", sdaiADB, &adb);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///	(Non-standard extension) sdiADB in C++ has an option to work without sdaiCreateEmptyADB and sdaiDeleteADB as shown in the table.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			Yes *		 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			Yes			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			Yes			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .			 .			 .
		///	sdaiUNICODE			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		///	Note: sdaiGetAttr, stdaiGetAttrBN, engiGetElement will success with any model data, except non-set($)
		///		  (Non-standard extensions) sdaiGetADBValue: sdaiADB is allowed and will success when sdaiGetADBTypePath is not NULL, returning ABD value has type path element removed.
		///
		///	Technically sdaiGetAttrBN will transform into the following call
		///		sdaiGetAttr(
		///				instance,
		///				sdaiGetAttrDefinition(
		///						sdaiGetInstanceType(
		///								instance
		///							),
		///						attributeName
		///					),
		///				valueType,
		///				value
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBN")]
		public static extern Int32 x86_sdaiGetAttrBN(Int32 instance, string attributeName, Int32 valueType, out bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBN")]
		public static extern Int64 x64_sdaiGetAttrBN(Int64 instance, string attributeName, Int64 valueType, out bool value);

		public static Int64 sdaiGetAttrBN(Int64 instance, string attributeName, Int64 valueType, out bool value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAttrBN((Int32)instance, attributeName, (Int32)valueType, out bool _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAttrBN(instance, attributeName, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBN")]
		public static extern Int32 x86_sdaiGetAttrBN(Int32 instance, string attributeName, Int32 valueType, out Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBN")]
		public static extern Int64 x64_sdaiGetAttrBN(Int64 instance, string attributeName, Int64 valueType, out Int64 value);

		public static Int64 sdaiGetAttrBN(Int64 instance, string attributeName, Int64 valueType, out Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAttrBN((Int32)instance, attributeName, (Int32)valueType, out Int32 _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAttrBN(instance, attributeName, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBN")]
		public static extern Int32 x86_sdaiGetAttrBN(Int32 instance, string attributeName, Int32 valueType, out double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBN")]
		public static extern Int64 x64_sdaiGetAttrBN(Int64 instance, string attributeName, Int64 valueType, out double value);

		public static Int64 sdaiGetAttrBN(Int64 instance, string attributeName, Int64 valueType, out double value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAttrBN((Int32)instance, attributeName, (Int32)valueType, out double _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAttrBN(instance, attributeName, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBN")]
		public static extern Int32 x86_sdaiGetAttrBN(Int32 instance, string attributeName, Int32 valueType, out IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBN")]
		public static extern Int64 x64_sdaiGetAttrBN(Int64 instance, string attributeName, Int64 valueType, out IntPtr value);

		public static Int64 sdaiGetAttrBN(Int64 instance, string attributeName, Int64 valueType, out IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAttrBN((Int32)instance, attributeName, (Int32)valueType, out IntPtr _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAttrBN(instance, attributeName, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBN")]
		public static extern Int32 x86_sdaiGetAttrBN(Int32 instance, byte[] attributeName, Int32 valueType, out bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBN")]
		public static extern Int64 x64_sdaiGetAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, out bool value);

		public static Int64 sdaiGetAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, out bool value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAttrBN((Int32)instance, attributeName, (Int32)valueType, out bool _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAttrBN(instance, attributeName, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBN")]
		public static extern Int32 x86_sdaiGetAttrBN(Int32 instance, byte[] attributeName, Int32 valueType, out Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBN")]
		public static extern Int64 x64_sdaiGetAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, out Int64 value);

		public static Int64 sdaiGetAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, out Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAttrBN((Int32)instance, attributeName, (Int32)valueType, out Int32 _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAttrBN(instance, attributeName, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBN")]
		public static extern Int32 x86_sdaiGetAttrBN(Int32 instance, byte[] attributeName, Int32 valueType, out double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBN")]
		public static extern Int64 x64_sdaiGetAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, out double value);

		public static Int64 sdaiGetAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, out double value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAttrBN((Int32)instance, attributeName, (Int32)valueType, out double _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAttrBN(instance, attributeName, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBN")]
		public static extern Int32 x86_sdaiGetAttrBN(Int32 instance, byte[] attributeName, Int32 valueType, out IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBN")]
		public static extern Int64 x64_sdaiGetAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, out IntPtr value);

		public static Int64 sdaiGetAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, out IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAttrBN((Int32)instance, attributeName, (Int32)valueType, out IntPtr _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAttrBN(instance, attributeName, valueType, out value);
			}
		}

		/// <summary>
		///		sdaiGetAttrBNUnicode                                    (https://rdf.bg/ifcdoc/CS64/sdaiGetAttrBNUnicode.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBNUnicode")]
		public static extern Int32 x86_sdaiGetAttrBNUnicode(Int32 instance, string attributeName, byte[] buffer, Int32 bufferLength);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBNUnicode")]
		public static extern Int64 x64_sdaiGetAttrBNUnicode(Int64 instance, string attributeName, byte[] buffer, Int64 bufferLength);

		public static Int64 sdaiGetAttrBNUnicode(Int64 instance, string attributeName, byte[] buffer, Int64 bufferLength)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAttrBNUnicode((Int32)instance, attributeName, buffer, (Int32)bufferLength);
				return _result;
			}
			else
			{
				return x64_sdaiGetAttrBNUnicode(instance, attributeName, buffer, bufferLength);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBNUnicode")]
		public static extern Int32 x86_sdaiGetAttrBNUnicode(Int32 instance, byte[] attributeName, byte[] buffer, Int32 bufferLength);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrBNUnicode")]
		public static extern Int64 x64_sdaiGetAttrBNUnicode(Int64 instance, byte[] attributeName, byte[] buffer, Int64 bufferLength);

		public static Int64 sdaiGetAttrBNUnicode(Int64 instance, byte[] attributeName, byte[] buffer, Int64 bufferLength)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAttrBNUnicode((Int32)instance, attributeName, buffer, (Int32)bufferLength);
				return _result;
			}
			else
			{
				return x64_sdaiGetAttrBNUnicode(instance, attributeName, buffer, bufferLength);
			}
		}

		/// <summary>
		///		sdaiGetStringAttrBN                                     (https://rdf.bg/ifcdoc/CS64/sdaiGetStringAttrBN.html)
		///
		///	This function is a specific version of sdaiGetAttrBN(..), where the valueType is sdaiSTRING.
		///	This call can be useful in case of specific programming languages that cannot map towards sdaiGetAttrBN(..) directly,
		///	this function is useless for languages as C, C++, C#, JAVA, VB.NET, Delphi and similar as they are able to map sdaiGetAttrBN(..) directly.
		///
		///	Technically sdaiGetStringAttrBN will transform into the following call
		///		char	* rValue = 0;
		///		sdaiGetAttr(
		///				instance,
		///				sdaiGetAttrDefinition(
		///						sdaiGetInstanceType(
		///								instance
		///							),
		///						attributeName
		///					),
		///				sdaiSTRING,
		///				&rValue
		///			);
		///		return	rValue;
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetStringAttrBN")]
		public static extern IntPtr x86_sdaiGetStringAttrBN(Int32 instance, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetStringAttrBN")]
		public static extern IntPtr x64_sdaiGetStringAttrBN(Int64 instance, string attributeName);

		public static IntPtr sdaiGetStringAttrBN(Int64 instance, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetStringAttrBN((Int32)instance, attributeName);
				return _result;
			}
			else
			{
				return x64_sdaiGetStringAttrBN(instance, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetStringAttrBN")]
		public static extern IntPtr x86_sdaiGetStringAttrBN(Int32 instance, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetStringAttrBN")]
		public static extern IntPtr x64_sdaiGetStringAttrBN(Int64 instance, byte[] attributeName);

		public static IntPtr sdaiGetStringAttrBN(Int64 instance, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetStringAttrBN((Int32)instance, attributeName);
				return _result;
			}
			else
			{
				return x64_sdaiGetStringAttrBN(instance, attributeName);
			}
		}

		/// <summary>
		///		sdaiGetInstanceAttrBN                                   (https://rdf.bg/ifcdoc/CS64/sdaiGetInstanceAttrBN.html)
		///
		///	This function is a specific version of sdaiGetAttrBN(..), where the valueType is sdaiINSTANCE.
		///	This call can be useful in case of specific programming languages that cannot map towards sdaiGetAttrBN(..) directly,
		///	this function is useless for languages as C, C++, C#, JAVA, VB.NET, Delphi and similar as they are able to map sdaiGetAttrBN(..) directly.
		///
		///	Technically sdaiGetInstanceAttrBN will transform into the following call
		///		SdaiInstance	inst = 0;
		///		sdaiGetAttr(
		///				instance,
		///				sdaiGetAttrDefinition(
		///						sdaiGetInstanceType(
		///								instance
		///							),
		///						attributeName
		///					),
		///				sdaiINSTANCE,
		///				&inst
		///			);
		///		return	inst;
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetInstanceAttrBN")]
		public static extern Int32 x86_sdaiGetInstanceAttrBN(Int32 instance, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetInstanceAttrBN")]
		public static extern Int64 x64_sdaiGetInstanceAttrBN(Int64 instance, string attributeName);

		public static Int64 sdaiGetInstanceAttrBN(Int64 instance, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetInstanceAttrBN((Int32)instance, attributeName);
				return _result;
			}
			else
			{
				return x64_sdaiGetInstanceAttrBN(instance, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetInstanceAttrBN")]
		public static extern Int32 x86_sdaiGetInstanceAttrBN(Int32 instance, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetInstanceAttrBN")]
		public static extern Int64 x64_sdaiGetInstanceAttrBN(Int64 instance, byte[] attributeName);

		public static Int64 sdaiGetInstanceAttrBN(Int64 instance, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetInstanceAttrBN((Int32)instance, attributeName);
				return _result;
			}
			else
			{
				return x64_sdaiGetInstanceAttrBN(instance, attributeName);
			}
		}

		/// <summary>
		///		sdaiGetAggregationAttrBN                                (https://rdf.bg/ifcdoc/CS64/sdaiGetAggregationAttrBN.html)
		///
		///	This function is a specific version of sdaiGetAttrBN(..), where the valueType is sdaiAGGR.
		///	This call can be useful in case of specific programming languages that cannot map towards sdaiGetAttrBN(..) directly,
		///	this function is useless for languages as C, C++, C#, JAVA, VB.NET, Delphi and similar as they are able to map sdaiGetAttrBN(..) directly.
		///
		///	Technically sdaiGetAggregationAttrBN will transform into the following call
		///		SdaiAggr	aggr = 0;
		///		sdaiGetAttr(
		///				instance,
		///				sdaiGetAttrDefinition(
		///						sdaiGetInstanceType(
		///								instance
		///							),
		///						attributeName
		///					),
		///				sdaiAGGR,
		///				&aggr
		///			);
		///		return	aggr;
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggregationAttrBN")]
		public static extern Int32 x86_sdaiGetAggregationAttrBN(Int32 instance, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggregationAttrBN")]
		public static extern Int64 x64_sdaiGetAggregationAttrBN(Int64 instance, string attributeName);

		public static Int64 sdaiGetAggregationAttrBN(Int64 instance, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAggregationAttrBN((Int32)instance, attributeName);
				return _result;
			}
			else
			{
				return x64_sdaiGetAggregationAttrBN(instance, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggregationAttrBN")]
		public static extern Int32 x86_sdaiGetAggregationAttrBN(Int32 instance, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggregationAttrBN")]
		public static extern Int64 x64_sdaiGetAggregationAttrBN(Int64 instance, byte[] attributeName);

		public static Int64 sdaiGetAggregationAttrBN(Int64 instance, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAggregationAttrBN((Int32)instance, attributeName);
				return _result;
			}
			else
			{
				return x64_sdaiGetAggregationAttrBN(instance, attributeName);
			}
		}

		/// <summary>
		///		sdaiGetAttrDefinition                                   (https://rdf.bg/ifcdoc/CS64/sdaiGetAttrDefinition.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrDefinition")]
		public static extern Int32 x86_sdaiGetAttrDefinition(Int32 entity, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrDefinition")]
		public static extern Int64 x64_sdaiGetAttrDefinition(Int64 entity, string attributeName);

		public static Int64 sdaiGetAttrDefinition(Int64 entity, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAttrDefinition((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_sdaiGetAttrDefinition(entity, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrDefinition")]
		public static extern Int32 x86_sdaiGetAttrDefinition(Int32 entity, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAttrDefinition")]
		public static extern Int64 x64_sdaiGetAttrDefinition(Int64 entity, byte[] attributeName);

		public static Int64 sdaiGetAttrDefinition(Int64 entity, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAttrDefinition((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_sdaiGetAttrDefinition(entity, attributeName);
			}
		}

		/// <summary>
		///		engiGetAttrTraits                                       (https://rdf.bg/ifcdoc/CS64/engiGetAttrTraits.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrTraits")]
		public static extern void x86_engiGetAttrTraits(Int32 attribute, out IntPtr name, Int32 definingEntity, out bool isExplicit, out bool isInverse, out enum_express_attr_type attrType, Int32 domainEntity, out Int32 aggregationDefinition, out bool isOptional);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrTraits")]
		public static extern void x64_engiGetAttrTraits(Int64 attribute, out IntPtr name, Int64 definingEntity, out bool isExplicit, out bool isInverse, out enum_express_attr_type attrType, Int64 domainEntity, out Int64 aggregationDefinition, out bool isOptional);

		public static void engiGetAttrTraits(Int64 attribute, out IntPtr name, Int64 definingEntity, out bool isExplicit, out bool isInverse, out enum_express_attr_type attrType, Int64 domainEntity, out Int64 aggregationDefinition, out bool isOptional)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiGetAttrTraits((Int32)attribute, out IntPtr _name, (Int32)definingEntity, out bool _isExplicit, out bool _isInverse, out enum_express_attr_type _attrType, (Int32)domainEntity, out Int32 _aggregationDefinition, out bool _isOptional);
				name = _name;
				isExplicit = _isExplicit;
				isInverse = _isInverse;
				attrType = _attrType;
				aggregationDefinition = _aggregationDefinition;
				isOptional = _isOptional;
			}
			else
			{
				x64_engiGetAttrTraits(attribute, out name, definingEntity, out isExplicit, out isInverse, out attrType, domainEntity, out aggregationDefinition, out isOptional);
			}
		}

		/// <summary>
		///		engiGetAttrName                                         (https://rdf.bg/ifcdoc/CS64/engiGetAttrName.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrName")]
		public static extern IntPtr x86_engiGetAttrName(Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrName")]
		public static extern IntPtr x64_engiGetAttrName(Int64 attribute);

		public static IntPtr engiGetAttrName(Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrName((Int32)attribute);
				return _result;
			}
			else
			{
				return x64_engiGetAttrName(attribute);
			}
		}

		/// <summary>
		///		engiGetAttrDefiningEntity                               (https://rdf.bg/ifcdoc/CS64/engiGetAttrDefiningEntity.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDefiningEntity")]
		public static extern Int32 x86_engiGetAttrDefiningEntity(Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDefiningEntity")]
		public static extern Int64 x64_engiGetAttrDefiningEntity(Int64 attribute);

		public static Int64 engiGetAttrDefiningEntity(Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrDefiningEntity((Int32)attribute);
				return _result;
			}
			else
			{
				return x64_engiGetAttrDefiningEntity(attribute);
			}
		}

		/// <summary>
		///		engiIsAttrExplicit                                      (https://rdf.bg/ifcdoc/CS64/engiIsAttrExplicit.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrExplicit")]
		public static extern bool x86_engiIsAttrExplicit(Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrExplicit")]
		public static extern bool x64_engiIsAttrExplicit(Int64 attribute);

		public static bool engiIsAttrExplicit(Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiIsAttrExplicit((Int32)attribute);
				return _result;
			}
			else
			{
				return x64_engiIsAttrExplicit(attribute);
			}
		}

		/// <summary>
		///		engiIsAttrExplicitBN                                    (https://rdf.bg/ifcdoc/CS64/engiIsAttrExplicitBN.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrExplicitBN")]
		public static extern bool x86_engiIsAttrExplicitBN(Int32 entity, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrExplicitBN")]
		public static extern bool x64_engiIsAttrExplicitBN(Int64 entity, string attributeName);

		public static bool engiIsAttrExplicitBN(Int64 entity, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiIsAttrExplicitBN((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiIsAttrExplicitBN(entity, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrExplicitBN")]
		public static extern bool x86_engiIsAttrExplicitBN(Int32 entity, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiIsAttrExplicitBN")]
		public static extern bool x64_engiIsAttrExplicitBN(Int64 entity, byte[] attributeName);

		public static bool engiIsAttrExplicitBN(Int64 entity, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiIsAttrExplicitBN((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiIsAttrExplicitBN(entity, attributeName);
			}
		}

		/// <summary>
		///		sdaiGetInstanceModel                                    (https://rdf.bg/ifcdoc/CS64/sdaiGetInstanceModel.html)
		///
		///	Returns the model based on an instance.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetInstanceModel")]
		public static extern Int32 x86_sdaiGetInstanceModel(Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetInstanceModel")]
		public static extern Int64 x64_sdaiGetInstanceModel(Int64 instance);

		public static Int64 sdaiGetInstanceModel(Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetInstanceModel((Int32)instance);
				return _result;
			}
			else
			{
				return x64_sdaiGetInstanceModel(instance);
			}
		}

		/// <summary>
		///		sdaiGetInstanceType                                     (https://rdf.bg/ifcdoc/CS64/sdaiGetInstanceType.html)
		///
		///	Returns the entity based on an instance.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetInstanceType")]
		public static extern Int32 x86_sdaiGetInstanceType(Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetInstanceType")]
		public static extern Int64 x64_sdaiGetInstanceType(Int64 instance);

		public static Int64 sdaiGetInstanceType(Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetInstanceType((Int32)instance);
				return _result;
			}
			else
			{
				return x64_sdaiGetInstanceType(instance);
			}
		}

		/// <summary>
		///		sdaiGetMemberCount                                      (https://rdf.bg/ifcdoc/CS64/sdaiGetMemberCount.html)
		///
		///	Returns the number of elements within an aggregation.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetMemberCount")]
		public static extern Int32 x86_sdaiGetMemberCount(Int32 aggregate);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetMemberCount")]
		public static extern Int64 x64_sdaiGetMemberCount(Int64 aggregate);

		public static Int64 sdaiGetMemberCount(Int64 aggregate)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetMemberCount((Int32)aggregate);
				return _result;
			}
			else
			{
				return x64_sdaiGetMemberCount(aggregate);
			}
		}

		/// <summary>
		///		sdaiIsKindOf                                            (https://rdf.bg/ifcdoc/CS64/sdaiIsKindOf.html)
		///
		///	This call checks if an instance is a type of a certain given entity.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsKindOf")]
		public static extern Int32 x86_sdaiIsKindOf(Int32 instance, Int32 entity);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsKindOf")]
		public static extern Int64 x64_sdaiIsKindOf(Int64 instance, Int64 entity);

		public static Int64 sdaiIsKindOf(Int64 instance, Int64 entity)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiIsKindOf((Int32)instance, (Int32)entity);
				return _result;
			}
			else
			{
				return x64_sdaiIsKindOf(instance, entity);
			}
		}

		/// <summary>
		///		sdaiIsKindOfBN                                          (https://rdf.bg/ifcdoc/CS64/sdaiIsKindOfBN.html)
		///
		///	This call checks if an instance is a type of a certain given entity.
		///
		///	Technically sdaiIsKindOfBN will transform into the following call
		///		sdaiIsKindOf(
		///				instance,
		///				sdaiGetEntity(
		///						engiGetEntityModel(
		///								sdaiGetInstanceType(
		///										instance
		///									)
		///							),
		///						entityName
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsKindOfBN")]
		public static extern Int32 x86_sdaiIsKindOfBN(Int32 instance, string entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsKindOfBN")]
		public static extern Int64 x64_sdaiIsKindOfBN(Int64 instance, string entityName);

		public static Int64 sdaiIsKindOfBN(Int64 instance, string entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiIsKindOfBN((Int32)instance, entityName);
				return _result;
			}
			else
			{
				return x64_sdaiIsKindOfBN(instance, entityName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsKindOfBN")]
		public static extern Int32 x86_sdaiIsKindOfBN(Int32 instance, byte[] entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsKindOfBN")]
		public static extern Int64 x64_sdaiIsKindOfBN(Int64 instance, byte[] entityName);

		public static Int64 sdaiIsKindOfBN(Int64 instance, byte[] entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiIsKindOfBN((Int32)instance, entityName);
				return _result;
			}
			else
			{
				return x64_sdaiIsKindOfBN(instance, entityName);
			}
		}

		/// <summary>
		///		engiGetAttrType                                         (https://rdf.bg/ifcdoc/CS64/engiGetAttrType.html)
		///
		///	Returns primitive SDAI data type for the attribute according to schema, e.g. sdaiINTEGER
		///
		///	In case of aggregation if will return base primitive type combined with engiTypeFlagAggr, e.g. sdaiINTEGER|engiTypeFlagAggr
		///
		///	For SELECT it will return sdaiINSTANCE if all options are instances or aggregation of instances, either sdaiADB
		///	In case of SELECT and sdaiINSTANCE, return value will be combined with engiTypeFlagAggrOption if some options are aggregation
		///	or engiTypeFlagAggr if all options are aggregations of instances
		///
		///	It works for explicit and inverse attributes
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrType")]
		public static extern Int32 x86_engiGetAttrType(Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrType")]
		public static extern Int64 x64_engiGetAttrType(Int64 attribute);

		public static Int64 engiGetAttrType(Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrType((Int32)attribute);
				return _result;
			}
			else
			{
				return x64_engiGetAttrType(attribute);
			}
		}

		/// <summary>
		///		engiGetAttrTypeBN                                       (https://rdf.bg/ifcdoc/CS64/engiGetAttrTypeBN.html)
		///
		///	Combines sdaiGetAttrDefinition and engiGetAttrType.
		///
		///	Technically engiGetAttrTypeBN will transform into the following call
		///		engiGetAttrType(
		///				sdaiGetAttrDefinition(
		///						entity,
		///						attributeName
		///					)
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrTypeBN")]
		public static extern Int32 x86_engiGetAttrTypeBN(Int32 entity, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrTypeBN")]
		public static extern Int64 x64_engiGetAttrTypeBN(Int64 entity, string attributeName);

		public static Int64 engiGetAttrTypeBN(Int64 entity, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrTypeBN((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiGetAttrTypeBN(entity, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrTypeBN")]
		public static extern Int32 x86_engiGetAttrTypeBN(Int32 entity, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrTypeBN")]
		public static extern Int64 x64_engiGetAttrTypeBN(Int64 entity, byte[] attributeName);

		public static Int64 engiGetAttrTypeBN(Int64 entity, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrTypeBN((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiGetAttrTypeBN(entity, attributeName);
			}
		}

		/// <summary>
		///		engiGetInstanceAttrType                                 (https://rdf.bg/ifcdoc/CS64/engiGetInstanceAttrType.html)
		///
		///	Returns SDAI type for actual data stored in the instance for the attribute.
		///	It may be primitive type, sdaiAGGR or sdaiADB.
		///	Returns 0 for $ and *.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetInstanceAttrType")]
		public static extern Int32 x86_engiGetInstanceAttrType(Int32 instance, Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetInstanceAttrType")]
		public static extern Int64 x64_engiGetInstanceAttrType(Int64 instance, Int64 attribute);

		public static Int64 engiGetInstanceAttrType(Int64 instance, Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetInstanceAttrType((Int32)instance, (Int32)attribute);
				return _result;
			}
			else
			{
				return x64_engiGetInstanceAttrType(instance, attribute);
			}
		}

		/// <summary>
		///		engiGetInstanceAttrTypeBN                               (https://rdf.bg/ifcdoc/CS64/engiGetInstanceAttrTypeBN.html)
		///
		///	Combines sdaiGetAttrDefinition and engiGetInstanceAttrType.
		///
		///	Technically engiGetInstanceAttrTypeBN will transform into the following call
		///		engiGetInstanceAttrType(
		///				instance,
		///				sdaiGetAttrDefinition(
		///						sdaiGetInstanceType(
		///								instance
		///							),
		///						attributeName
		///					)
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetInstanceAttrTypeBN")]
		public static extern Int32 x86_engiGetInstanceAttrTypeBN(Int32 instance, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetInstanceAttrTypeBN")]
		public static extern Int64 x64_engiGetInstanceAttrTypeBN(Int64 instance, string attributeName);

		public static Int64 engiGetInstanceAttrTypeBN(Int64 instance, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetInstanceAttrTypeBN((Int32)instance, attributeName);
				return _result;
			}
			else
			{
				return x64_engiGetInstanceAttrTypeBN(instance, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetInstanceAttrTypeBN")]
		public static extern Int32 x86_engiGetInstanceAttrTypeBN(Int32 instance, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetInstanceAttrTypeBN")]
		public static extern Int64 x64_engiGetInstanceAttrTypeBN(Int64 instance, byte[] attributeName);

		public static Int64 engiGetInstanceAttrTypeBN(Int64 instance, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetInstanceAttrTypeBN((Int32)instance, attributeName);
				return _result;
			}
			else
			{
				return x64_engiGetInstanceAttrTypeBN(instance, attributeName);
			}
		}

		/// <summary>
		///		sdaiIsInstanceOf                                        (https://rdf.bg/ifcdoc/CS64/sdaiIsInstanceOf.html)
		///
		///	This call checks if an instance is an exact instance of a given entity.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsInstanceOf")]
		public static extern Int32 x86_sdaiIsInstanceOf(Int32 instance, Int32 entity);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsInstanceOf")]
		public static extern Int64 x64_sdaiIsInstanceOf(Int64 instance, Int64 entity);

		public static Int64 sdaiIsInstanceOf(Int64 instance, Int64 entity)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiIsInstanceOf((Int32)instance, (Int32)entity);
				return _result;
			}
			else
			{
				return x64_sdaiIsInstanceOf(instance, entity);
			}
		}

		/// <summary>
		///		sdaiIsInstanceOfBN                                      (https://rdf.bg/ifcdoc/CS64/sdaiIsInstanceOfBN.html)
		///
		///	This call checks if an instance is an exact instance of a given entity.
		///
		///	Technically sdaiIsInstanceOfBN will transform into the following call
		///		sdaiIsInstanceOf(
		///				instance,
		///				sdaiGetEntity(
		///						engiGetEntityModel(
		///								sdaiGetInstanceType(
		///										instance
		///									)
		///							),
		///						entityName
		///					)
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsInstanceOfBN")]
		public static extern Int32 x86_sdaiIsInstanceOfBN(Int32 instance, string entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsInstanceOfBN")]
		public static extern Int64 x64_sdaiIsInstanceOfBN(Int64 instance, string entityName);

		public static Int64 sdaiIsInstanceOfBN(Int64 instance, string entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiIsInstanceOfBN((Int32)instance, entityName);
				return _result;
			}
			else
			{
				return x64_sdaiIsInstanceOfBN(instance, entityName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsInstanceOfBN")]
		public static extern Int32 x86_sdaiIsInstanceOfBN(Int32 instance, byte[] entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsInstanceOfBN")]
		public static extern Int64 x64_sdaiIsInstanceOfBN(Int64 instance, byte[] entityName);

		public static Int64 sdaiIsInstanceOfBN(Int64 instance, byte[] entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiIsInstanceOfBN((Int32)instance, entityName);
				return _result;
			}
			else
			{
				return x64_sdaiIsInstanceOfBN(instance, entityName);
			}
		}

		/// <summary>
		///		sdaiIsEqual                                             (https://rdf.bg/ifcdoc/CS64/sdaiIsEqual.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsEqual")]
		public static extern byte x86_sdaiIsEqual(Int32 instanceI, Int32 instanceII);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsEqual")]
		public static extern byte x64_sdaiIsEqual(Int64 instanceI, Int64 instanceII);

		public static byte sdaiIsEqual(Int64 instanceI, Int64 instanceII)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiIsEqual((Int32)instanceI, (Int32)instanceII);
				return _result;
			}
			else
			{
				return x64_sdaiIsEqual(instanceI, instanceII);
			}
		}

		/// <summary>
		///		sdaiValidateAttribute                                   (https://rdf.bg/ifcdoc/CS64/sdaiValidateAttribute.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiValidateAttribute")]
		public static extern Int32 x86_sdaiValidateAttribute(Int32 instance, Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiValidateAttribute")]
		public static extern Int64 x64_sdaiValidateAttribute(Int64 instance, Int64 attribute);

		public static Int64 sdaiValidateAttribute(Int64 instance, Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiValidateAttribute((Int32)instance, (Int32)attribute);
				return _result;
			}
			else
			{
				return x64_sdaiValidateAttribute(instance, attribute);
			}
		}

		/// <summary>
		///		sdaiValidateAttributeBN                                 (https://rdf.bg/ifcdoc/CS64/sdaiValidateAttributeBN.html)
		///
		///	Technically it will transform into the following call
		///		sdaiValidateAttribute(
		///				instance,
		///				sdaiGetAttrDefinition(
		///						sdaiGetInstanceType(
		///								instance
		///							),
		///						attributeName
		///					)
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiValidateAttributeBN")]
		public static extern Int32 x86_sdaiValidateAttributeBN(Int32 instance, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiValidateAttributeBN")]
		public static extern Int64 x64_sdaiValidateAttributeBN(Int64 instance, string attributeName);

		public static Int64 sdaiValidateAttributeBN(Int64 instance, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiValidateAttributeBN((Int32)instance, attributeName);
				return _result;
			}
			else
			{
				return x64_sdaiValidateAttributeBN(instance, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiValidateAttributeBN")]
		public static extern Int32 x86_sdaiValidateAttributeBN(Int32 instance, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiValidateAttributeBN")]
		public static extern Int64 x64_sdaiValidateAttributeBN(Int64 instance, byte[] attributeName);

		public static Int64 sdaiValidateAttributeBN(Int64 instance, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiValidateAttributeBN((Int32)instance, attributeName);
				return _result;
			}
			else
			{
				return x64_sdaiValidateAttributeBN(instance, attributeName);
			}
		}

		/// <summary>
		///		engiGetInstanceClassInfo                                (https://rdf.bg/ifcdoc/CS64/engiGetInstanceClassInfo.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetInstanceClassInfo")]
		public static extern IntPtr x86_engiGetInstanceClassInfo(Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetInstanceClassInfo")]
		public static extern IntPtr x64_engiGetInstanceClassInfo(Int64 instance);

		public static IntPtr engiGetInstanceClassInfo(Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetInstanceClassInfo((Int32)instance);
				return _result;
			}
			else
			{
				return x64_engiGetInstanceClassInfo(instance);
			}
		}

		/// <summary>
		///		engiGetInstanceClassInfoUC                              (https://rdf.bg/ifcdoc/CS64/engiGetInstanceClassInfoUC.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetInstanceClassInfoUC")]
		public static extern IntPtr x86_engiGetInstanceClassInfoUC(Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetInstanceClassInfoUC")]
		public static extern IntPtr x64_engiGetInstanceClassInfoUC(Int64 instance);

		public static IntPtr engiGetInstanceClassInfoUC(Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetInstanceClassInfoUC((Int32)instance);
				return _result;
			}
			else
			{
				return x64_engiGetInstanceClassInfoUC(instance);
			}
		}

		/// <summary>
		///		engiGetInstanceMetaInfo                                 (https://rdf.bg/ifcdoc/CS64/engiGetInstanceMetaInfo.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetInstanceMetaInfo")]
		public static extern Int32 x86_engiGetInstanceMetaInfo(Int32 instance, out Int32 localId, out IntPtr entityName, out IntPtr entityNameUC);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetInstanceMetaInfo")]
		public static extern Int64 x64_engiGetInstanceMetaInfo(Int64 instance, out Int64 localId, out IntPtr entityName, out IntPtr entityNameUC);

		public static Int64 engiGetInstanceMetaInfo(Int64 instance, out Int64 localId, out IntPtr entityName, out IntPtr entityNameUC)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetInstanceMetaInfo((Int32)instance, out Int32 _localId, out IntPtr _entityName, out IntPtr _entityNameUC);
				localId = _localId;
				entityName = _entityName;
				entityNameUC = _entityNameUC;
				return _result;
			}
			else
			{
				return x64_engiGetInstanceMetaInfo(instance, out localId, out entityName, out entityNameUC);
			}
		}

		/// <summary>
		///		sdaiFindInstanceUsers                                   (https://rdf.bg/ifcdoc/CS64/sdaiFindInstanceUsers.html)
		///
		///	The function returns the identifiers of all the entity instances in the defined domain
		///	that reference the specified entity instance.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiFindInstanceUsers")]
		public static extern Int32 x86_sdaiFindInstanceUsers(Int32 instance, Int32 domain, Int32 resultList);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiFindInstanceUsers")]
		public static extern Int64 x64_sdaiFindInstanceUsers(Int64 instance, Int64 domain, Int64 resultList);

		public static Int64 sdaiFindInstanceUsers(Int64 instance, Int64 domain, Int64 resultList)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiFindInstanceUsers((Int32)instance, (Int32)domain, (Int32)resultList);
				return _result;
			}
			else
			{
				return x64_sdaiFindInstanceUsers(instance, domain, resultList);
			}
		}

		/// <summary>
		///		sdaiFindInstanceUsedIn                                  (https://rdf.bg/ifcdoc/CS64/sdaiFindInstanceUsedIn.html)
		///
		///	The function returns the identifiers of all the entity instances in the defined domain
		///	that reference the specified entity instance by the specified attribute (role).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiFindInstanceUsedIn")]
		public static extern Int32 x86_sdaiFindInstanceUsedIn(Int32 instance, Int32 role, Int32 domain, Int32 resultList);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiFindInstanceUsedIn")]
		public static extern Int64 x64_sdaiFindInstanceUsedIn(Int64 instance, Int64 role, Int64 domain, Int64 resultList);

		public static Int64 sdaiFindInstanceUsedIn(Int64 instance, Int64 role, Int64 domain, Int64 resultList)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiFindInstanceUsedIn((Int32)instance, (Int32)role, (Int32)domain, (Int32)resultList);
				return _result;
			}
			else
			{
				return x64_sdaiFindInstanceUsedIn(instance, role, domain, resultList);
			}
		}

		/// <summary>
		///		sdaiFindInstanceUsedInBN                                (https://rdf.bg/ifcdoc/CS64/sdaiFindInstanceUsedInBN.html)
		///
		///	The function returns the identifiers of all the entity instances in the defined domain
		///	that reference the specified entity instance by the specified attribute (with roleName).
		///
		///	Technically sdaiFindInstanceUsedInBN will transform into the following call
		///		sdaiFindInstanceUsedIn(
		///				instance,
		///				sdaiGetAttrDefinition(
		///						sdaiGetInstanceType(
		///								instance
		///							),
		///						roleName
		///					),
		///				domain,
		///				resultList
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiFindInstanceUsedInBN")]
		public static extern Int32 x86_sdaiFindInstanceUsedInBN(Int32 instance, string roleName, Int32 domain, Int32 resultList);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiFindInstanceUsedInBN")]
		public static extern Int64 x64_sdaiFindInstanceUsedInBN(Int64 instance, string roleName, Int64 domain, Int64 resultList);

		public static Int64 sdaiFindInstanceUsedInBN(Int64 instance, string roleName, Int64 domain, Int64 resultList)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiFindInstanceUsedInBN((Int32)instance, roleName, (Int32)domain, (Int32)resultList);
				return _result;
			}
			else
			{
				return x64_sdaiFindInstanceUsedInBN(instance, roleName, domain, resultList);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiFindInstanceUsedInBN")]
		public static extern Int32 x86_sdaiFindInstanceUsedInBN(Int32 instance, byte[] roleName, Int32 domain, Int32 resultList);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiFindInstanceUsedInBN")]
		public static extern Int64 x64_sdaiFindInstanceUsedInBN(Int64 instance, byte[] roleName, Int64 domain, Int64 resultList);

		public static Int64 sdaiFindInstanceUsedInBN(Int64 instance, byte[] roleName, Int64 domain, Int64 resultList)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiFindInstanceUsedInBN((Int32)instance, roleName, (Int32)domain, (Int32)resultList);
				return _result;
			}
			else
			{
				return x64_sdaiFindInstanceUsedInBN(instance, roleName, domain, resultList);
			}
		}

        //
        //  Instance Writing API Calls
        //

		/// <summary>
		///		sdaiPrepend                                             (https://rdf.bg/ifcdoc/CS64/sdaiPrepend.html)
		///
		///	valueType argument to specify what type of data caller wants to put
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiPrepend, and it works similarly for all put-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiPrepend but valid for all put-functions)
		///
		///	valueType				C/C++														C#
		///
		///	sdaiINTEGER				int_t val = 123;											int_t val = 123;
		///							sdaiPrepend (aggregate, sdaiINTEGER, &val);					ifcengine.sdaiPrepend (aggregate, ifcengine.sdaiINTEGER, ref val);
		///
		///	sdaiREAL or sdaiNUMBER	double val = 123.456;										double val = 123.456;
		///							sdaiPrepend (aggregate, sdaiREAL, &val);					ifcengine.sdaiPrepend (aggregate, ifcengine.sdaiREAL, ref val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val = sdaiTRUE;									bool val = true;
		///							sdaiPrepend (aggregate, sdaiBOOLEAN, &val);					ifcengine.sdaiPrepend (aggregate, ifcengine.sdaiBOOLEAN, ref val);
		///
		///	sdaiLOGICAL				const TCHAR* val = "U";										string val = "U";
		///							sdaiPrepend (aggregate, sdaiLOGICAL, val);					ifcengine.sdaiPrepend (aggregate, ifcengine.sdaiLOGICAL, val);
		///
		///	sdaiENUM				const TCHAR* val = "NOTDEFINED";							string val = "NOTDEFINED";
		///							sdaiPrepend (aggregate, sdaiENUM, val);						ifcengine.sdaiPrepend (aggregate, ifcengine.sdaiENUM, val);
		///
		///	sdaiBINARY				const TCHAR* val = "0123456ABC";							string val = "0123456ABC";
		///							sdaiPrepend (aggregate, sdaiBINARY, val);					ifcengine.sdaiPrepend (aggregate, ifcengine.sdaiBINARY, val);
		///
		///	sdaiSTRING				const char* val = "My Simple String";						string val = "My Simple String";
		///							sdaiPrepend (aggregate, sdaiSTRING, val);					ifcengine.sdaiPrepend (aggregate, ifcengine.sdaiSTRING, val);
		///
		///	sdaiUNICODE				const wchar_t* val = L"Any Unicode String";					string val = "Any Unicode String";
		///							sdaiPrepend (aggregate, sdaiUNICODE, val);					ifcengine.sdaiPrepend (aggregate, ifcengine.sdaiUNICODE, val);
		///
		///	sdaiEXPRESSSTRING		const char* val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";	string val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";
		///							sdaiPrepend (aggregate, sdaiEXPRESSSTRING, val);			ifcengine.sdaiPrepend (aggregate, ifcengine.sdaiEXPRESSSTRING, val);
		///
		///	sdaiINSTANCE			SdaiInstance val = sdaiCreateInstanceBN (model, "IFCSITE");	int_t val = ifcengine.sdaiCreateInstanceBN (model, "IFCSITE");
		///							sdaiPrepend (aggregate, sdaiINSTANCE, val);					ifcengine.sdaiPrepend (aggregate, ifcengine.sdaiINSTANCE, val);
		///
		///	sdaiAGGR				SdaiAggr val = sdaiCreateAggr (inst, 0);					int_t val = sdaiCreateAggr (inst, 0);
		///							sdaiPutAttr (val, sdaiINSTANCE, inst);						ifcengine.sdaiPutAttr (val, ifcengine.sdaiINSTANCE, inst);
		///							sdaiPrepend (aggregate, sdaiAGGR, val);						ifcengine.sdaiPrepend (aggregate, ifcengine.sdaiAGGR, val);
		///
		///	sdaiADB					int_t integerValue = 123;									int_t integerValue = 123;	
		///							SdaiADB val = sdaiCreateADB (sdaiINTEGER, &integerValue);	int_t val = ifcengine.sdaiCreateADB (ifcengine.sdaiINTEGER, ref integerValue);
		///							sdaiPutADBTypePath (val, 1, "IFCINTEGER");					ifcengine.sdaiPutADBTypePath (val, 1, "IFCINTEGER");
		///							sdaiPrepend (aggregate, sdaiADB, val);						ifcengine.sdaiPrepend (aggregate, ifcengine.sdaiADB, val);	
		///							sdaiDeleteADB (val);										ifcengine.sdaiDeleteADB (val);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///	(Non-standard extension) sdiADB in C++ has an option to work without sdaiCreateEmptyADB and sdaiDeleteADB as shown in the table.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			 .			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			 .			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			 . 			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiUNICODE			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPrepend")]
		public static extern void x86_sdaiPrepend(Int32 aggregate, Int32 valueType, ref bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPrepend")]
		public static extern void x64_sdaiPrepend(Int64 aggregate, Int64 valueType, ref bool value);

		public static void sdaiPrepend(Int64 aggregate, Int64 valueType, ref bool value)
		{
			if (IntPtr.Size == 4)
			{
				bool _value = (bool)value;
				x86_sdaiPrepend((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPrepend(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPrepend")]
		public static extern void x86_sdaiPrepend(Int32 aggregate, Int32 valueType, ref Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPrepend")]
		public static extern void x64_sdaiPrepend(Int64 aggregate, Int64 valueType, ref Int64 value);

		public static void sdaiPrepend(Int64 aggregate, Int64 valueType, ref Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				Int32 _value = (Int32)value;
				x86_sdaiPrepend((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPrepend(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPrepend")]
		public static extern void x86_sdaiPrepend(Int32 aggregate, Int32 valueType, Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPrepend")]
		public static extern void x64_sdaiPrepend(Int64 aggregate, Int64 valueType, Int64 value);

		public static void sdaiPrepend(Int64 aggregate, Int64 valueType, Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPrepend((Int32)aggregate, (Int32)valueType, (Int32)value);
			}
			else
			{
				x64_sdaiPrepend(aggregate, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPrepend")]
		public static extern void x86_sdaiPrepend(Int32 aggregate, Int32 valueType, ref double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPrepend")]
		public static extern void x64_sdaiPrepend(Int64 aggregate, Int64 valueType, ref double value);

		public static void sdaiPrepend(Int64 aggregate, Int64 valueType, ref double value)
		{
			if (IntPtr.Size == 4)
			{
				double _value = (double)value;
				x86_sdaiPrepend((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPrepend(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPrepend")]
		public static extern void x86_sdaiPrepend(Int32 aggregate, Int32 valueType, ref IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPrepend")]
		public static extern void x64_sdaiPrepend(Int64 aggregate, Int64 valueType, ref IntPtr value);

		public static void sdaiPrepend(Int64 aggregate, Int64 valueType, ref IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				IntPtr _value = (IntPtr)value;
				x86_sdaiPrepend((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPrepend(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPrepend")]
		public static extern void x86_sdaiPrepend(Int32 aggregate, Int32 valueType, byte[] value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPrepend")]
		public static extern void x64_sdaiPrepend(Int64 aggregate, Int64 valueType, byte[] value);

		public static void sdaiPrepend(Int64 aggregate, Int64 valueType, byte[] value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPrepend((Int32)aggregate, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiPrepend(aggregate, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPrepend")]
		public static extern void x86_sdaiPrepend(Int32 aggregate, Int32 valueType, string value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPrepend")]
		public static extern void x64_sdaiPrepend(Int64 aggregate, Int64 valueType, string value);

		public static void sdaiPrepend(Int64 aggregate, Int64 valueType, string value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPrepend((Int32)aggregate, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiPrepend(aggregate, valueType, value);
			}
		}

		/// <summary>
		///		sdaiAppend                                              (https://rdf.bg/ifcdoc/CS64/sdaiAppend.html)
		///
		///	valueType argument to specify what type of data caller wants to put
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiAppend, and it works similarly for all put-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiAppend but valid for all put-functions)
		///
		///	valueType				C/C++														C#
		///
		///	sdaiINTEGER				int_t val = 123;											int_t val = 123;
		///							sdaiAppend (aggregate, sdaiINTEGER, &val);					ifcengine.sdaiAppend (aggregate, ifcengine.sdaiINTEGER, ref val);
		///
		///	sdaiREAL or sdaiNUMBER	double val = 123.456;										double val = 123.456;
		///							sdaiAppend (aggregate, sdaiREAL, &val);						ifcengine.sdaiAppend (aggregate, ifcengine.sdaiREAL, ref val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val = sdaiTRUE;									bool val = true;
		///							sdaiAppend (aggregate, sdaiBOOLEAN, &val);					ifcengine.sdaiAppend (aggregate, ifcengine.sdaiBOOLEAN, ref val);
		///
		///	sdaiLOGICAL				const TCHAR* val = "U";										string val = "U";
		///							sdaiAppend (aggregate, sdaiLOGICAL, val);					ifcengine.sdaiAppend (aggregate, ifcengine.sdaiLOGICAL, val);
		///
		///	sdaiENUM				const TCHAR* val = "NOTDEFINED";							string val = "NOTDEFINED";
		///							sdaiAppend (aggregate, sdaiENUM, val);						ifcengine.sdaiAppend (aggregate, ifcengine.sdaiENUM, val);
		///
		///	sdaiBINARY				const TCHAR* val = "0123456ABC";							string val = "0123456ABC";
		///							sdaiAppend (aggregate, sdaiBINARY, val);					ifcengine.sdaiAppend (aggregate, ifcengine.sdaiBINARY, val);
		///
		///	sdaiSTRING				const char* val = "My Simple String";						string val = "My Simple String";
		///							sdaiAppend (aggregate, sdaiSTRING, val);					ifcengine.sdaiAppend (aggregate, ifcengine.sdaiSTRING, val);
		///
		///	sdaiUNICODE				const wchar_t* val = L"Any Unicode String";					string val = "Any Unicode String";
		///							sdaiAppend (aggregate, sdaiUNICODE, val);					ifcengine.sdaiAppend (aggregate, ifcengine.sdaiUNICODE, val);
		///
		///	sdaiEXPRESSSTRING		const char* val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";	string val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";
		///							sdaiAppend (aggregate, sdaiEXPRESSSTRING, val);				ifcengine.sdaiAppend (aggregate, ifcengine.sdaiEXPRESSSTRING, val);
		///
		///	sdaiINSTANCE			SdaiInstance val = sdaiCreateInstanceBN (model, "IFCSITE");	int_t val = ifcengine.sdaiCreateInstanceBN (model, "IFCSITE");
		///							sdaiAppend (aggregate, sdaiINSTANCE, val);					ifcengine.sdaiAppend (aggregate, ifcengine.sdaiINSTANCE, val);
		///
		///	sdaiAGGR				SdaiAggr val = sdaiCreateAggr (inst, 0);					int_t val = sdaiCreateAggr (inst, 0);
		///							sdaiPutAttr (val, sdaiINSTANCE, inst);						ifcengine.sdaiPutAttr (val, ifcengine.sdaiINSTANCE, inst);
		///							sdaiAppend (aggregate, sdaiAGGR, val);						ifcengine.sdaiAppend (aggregate, ifcengine.sdaiAGGR, val);
		///
		///	sdaiADB					int_t integerValue = 123;									int_t integerValue = 123;	
		///							SdaiADB val = sdaiCreateADB (sdaiINTEGER, &integerValue);	int_t val = ifcengine.sdaiCreateADB (ifcengine.sdaiINTEGER, ref integerValue);
		///							sdaiPutADBTypePath (val, 1, "IFCINTEGER");					ifcengine.sdaiPutADBTypePath (val, 1, "IFCINTEGER");
		///							sdaiAppend (aggregate, sdaiADB, val);						ifcengine.sdaiAppend (aggregate, ifcengine.sdaiADB, val);	
		///							sdaiDeleteADB (val);										ifcengine.sdaiDeleteADB (val);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///	(Non-standard extension) sdiADB in C++ has an option to work without sdaiCreateEmptyADB and sdaiDeleteADB as shown in the table.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			 .			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			 .			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			 . 			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiUNICODE			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAppend")]
		public static extern void x86_sdaiAppend(Int32 aggregate, Int32 valueType, ref bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAppend")]
		public static extern void x64_sdaiAppend(Int64 aggregate, Int64 valueType, ref bool value);

		public static void sdaiAppend(Int64 aggregate, Int64 valueType, ref bool value)
		{
			if (IntPtr.Size == 4)
			{
				bool _value = (bool)value;
				x86_sdaiAppend((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiAppend(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAppend")]
		public static extern void x86_sdaiAppend(Int32 aggregate, Int32 valueType, ref Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAppend")]
		public static extern void x64_sdaiAppend(Int64 aggregate, Int64 valueType, ref Int64 value);

		public static void sdaiAppend(Int64 aggregate, Int64 valueType, ref Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				Int32 _value = (Int32)value;
				x86_sdaiAppend((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiAppend(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAppend")]
		public static extern void x86_sdaiAppend(Int32 aggregate, Int32 valueType, Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAppend")]
		public static extern void x64_sdaiAppend(Int64 aggregate, Int64 valueType, Int64 value);

		public static void sdaiAppend(Int64 aggregate, Int64 valueType, Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiAppend((Int32)aggregate, (Int32)valueType, (Int32)value);
			}
			else
			{
				x64_sdaiAppend(aggregate, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAppend")]
		public static extern void x86_sdaiAppend(Int32 aggregate, Int32 valueType, ref double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAppend")]
		public static extern void x64_sdaiAppend(Int64 aggregate, Int64 valueType, ref double value);

		public static void sdaiAppend(Int64 aggregate, Int64 valueType, ref double value)
		{
			if (IntPtr.Size == 4)
			{
				double _value = (double)value;
				x86_sdaiAppend((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiAppend(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAppend")]
		public static extern void x86_sdaiAppend(Int32 aggregate, Int32 valueType, ref IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAppend")]
		public static extern void x64_sdaiAppend(Int64 aggregate, Int64 valueType, ref IntPtr value);

		public static void sdaiAppend(Int64 aggregate, Int64 valueType, ref IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				IntPtr _value = (IntPtr)value;
				x86_sdaiAppend((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiAppend(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAppend")]
		public static extern void x86_sdaiAppend(Int32 aggregate, Int32 valueType, byte[] value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAppend")]
		public static extern void x64_sdaiAppend(Int64 aggregate, Int64 valueType, byte[] value);

		public static void sdaiAppend(Int64 aggregate, Int64 valueType, byte[] value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiAppend((Int32)aggregate, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiAppend(aggregate, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAppend")]
		public static extern void x86_sdaiAppend(Int32 aggregate, Int32 valueType, string value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAppend")]
		public static extern void x64_sdaiAppend(Int64 aggregate, Int64 valueType, string value);

		public static void sdaiAppend(Int64 aggregate, Int64 valueType, string value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiAppend((Int32)aggregate, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiAppend(aggregate, valueType, value);
			}
		}

		/// <summary>
		///		sdaiAdd                                                 (https://rdf.bg/ifcdoc/CS64/sdaiAdd.html)
		///
		///	valueType argument to specify what type of data caller wants to put
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiAdd, and it works similarly for all put-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiAdd but valid for all put-functions)
		///
		///	valueType				C/C++														C#
		///
		///	sdaiINTEGER				int_t val = 123;											int_t val = 123;
		///							sdaiAdd (aggregate, sdaiINTEGER, &val);						ifcengine.sdaiAdd (aggregate, ifcengine.sdaiINTEGER, ref val);
		///
		///	sdaiREAL or sdaiNUMBER	double val = 123.456;										double val = 123.456;
		///							sdaiAdd (aggregate, sdaiREAL, &val);						ifcengine.sdaiAdd (aggregate, ifcengine.sdaiREAL, ref val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val = sdaiTRUE;									bool val = true;
		///							sdaiAdd (aggregate, sdaiBOOLEAN, &val);						ifcengine.sdaiAdd (aggregate, ifcengine.sdaiBOOLEAN, ref val);
		///
		///	sdaiLOGICAL				const TCHAR* val = "U";										string val = "U";
		///							sdaiAdd (aggregate, sdaiLOGICAL, val);						ifcengine.sdaiAdd (aggregate, ifcengine.sdaiLOGICAL, val);
		///
		///	sdaiENUM				const TCHAR* val = "NOTDEFINED";							string val = "NOTDEFINED";
		///							sdaiAdd (aggregate, sdaiENUM, val);							ifcengine.sdaiAdd (aggregate, ifcengine.sdaiENUM, val);
		///
		///	sdaiBINARY				const TCHAR* val = "0123456ABC";							string val = "0123456ABC";
		///							sdaiAdd (aggregate, sdaiBINARY, val);						ifcengine.sdaiAdd (aggregate, ifcengine.sdaiBINARY, val);
		///
		///	sdaiSTRING				const char* val = "My Simple String";						string val = "My Simple String";
		///							sdaiAdd (aggregate, sdaiSTRING, val);						ifcengine.sdaiAdd (aggregate, ifcengine.sdaiSTRING, val);
		///
		///	sdaiUNICODE				const wchar_t* val = L"Any Unicode String";					string val = "Any Unicode String";
		///							sdaiAdd (aggregate, sdaiUNICODE, val);						ifcengine.sdaiAdd (aggregate, ifcengine.sdaiUNICODE, val);
		///
		///	sdaiEXPRESSSTRING		const char* val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";	string val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";
		///							sdaiAdd (aggregate, sdaiEXPRESSSTRING, val);				ifcengine.sdaiAdd (aggregate, ifcengine.sdaiEXPRESSSTRING, val);
		///
		///	sdaiINSTANCE			SdaiInstance val = sdaiCreateInstanceBN (model, "IFCSITE");	int_t val = ifcengine.sdaiCreateInstanceBN (model, "IFCSITE");
		///							sdaiAdd (aggregate, sdaiINSTANCE, val);						ifcengine.sdaiAdd (aggregate, ifcengine.sdaiINSTANCE, val);
		///
		///	sdaiAGGR				SdaiAggr val = sdaiCreateAggr (inst, 0);					int_t val = sdaiCreateAggr (inst, 0);
		///							sdaiPutAttr (val, sdaiINSTANCE, inst);						ifcengine.sdaiPutAttr (val, ifcengine.sdaiINSTANCE, inst);
		///							sdaiAdd (aggregate, sdaiAGGR, val);							ifcengine.sdaiAdd (aggregate, ifcengine.sdaiAGGR, val);
		///
		///	sdaiADB					int_t integerValue = 123;									int_t integerValue = 123;	
		///							SdaiADB val = sdaiCreateADB (sdaiINTEGER, &integerValue);	int_t val = ifcengine.sdaiCreateADB (ifcengine.sdaiINTEGER, ref integerValue);
		///							sdaiPutADBTypePath (val, 1, "IFCINTEGER");					ifcengine.sdaiPutADBTypePath (val, 1, "IFCINTEGER");
		///							sdaiAdd (aggregate, sdaiADB, val);							ifcengine.sdaiAdd (aggregate, ifcengine.sdaiADB, val);	
		///							sdaiDeleteADB (val);										ifcengine.sdaiDeleteADB (val);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///	(Non-standard extension) sdiADB in C++ has an option to work without sdaiCreateEmptyADB and sdaiDeleteADB as shown in the table.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			 .			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			 .			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			 . 			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiUNICODE			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAdd")]
		public static extern void x86_sdaiAdd(Int32 aggregate, Int32 valueType, ref bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAdd")]
		public static extern void x64_sdaiAdd(Int64 aggregate, Int64 valueType, ref bool value);

		public static void sdaiAdd(Int64 aggregate, Int64 valueType, ref bool value)
		{
			if (IntPtr.Size == 4)
			{
				bool _value = (bool)value;
				x86_sdaiAdd((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiAdd(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAdd")]
		public static extern void x86_sdaiAdd(Int32 aggregate, Int32 valueType, ref Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAdd")]
		public static extern void x64_sdaiAdd(Int64 aggregate, Int64 valueType, ref Int64 value);

		public static void sdaiAdd(Int64 aggregate, Int64 valueType, ref Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				Int32 _value = (Int32)value;
				x86_sdaiAdd((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiAdd(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAdd")]
		public static extern void x86_sdaiAdd(Int32 aggregate, Int32 valueType, Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAdd")]
		public static extern void x64_sdaiAdd(Int64 aggregate, Int64 valueType, Int64 value);

		public static void sdaiAdd(Int64 aggregate, Int64 valueType, Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiAdd((Int32)aggregate, (Int32)valueType, (Int32)value);
			}
			else
			{
				x64_sdaiAdd(aggregate, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAdd")]
		public static extern void x86_sdaiAdd(Int32 aggregate, Int32 valueType, ref double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAdd")]
		public static extern void x64_sdaiAdd(Int64 aggregate, Int64 valueType, ref double value);

		public static void sdaiAdd(Int64 aggregate, Int64 valueType, ref double value)
		{
			if (IntPtr.Size == 4)
			{
				double _value = (double)value;
				x86_sdaiAdd((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiAdd(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAdd")]
		public static extern void x86_sdaiAdd(Int32 aggregate, Int32 valueType, ref IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAdd")]
		public static extern void x64_sdaiAdd(Int64 aggregate, Int64 valueType, ref IntPtr value);

		public static void sdaiAdd(Int64 aggregate, Int64 valueType, ref IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				IntPtr _value = (IntPtr)value;
				x86_sdaiAdd((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiAdd(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAdd")]
		public static extern void x86_sdaiAdd(Int32 aggregate, Int32 valueType, byte[] value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAdd")]
		public static extern void x64_sdaiAdd(Int64 aggregate, Int64 valueType, byte[] value);

		public static void sdaiAdd(Int64 aggregate, Int64 valueType, byte[] value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiAdd((Int32)aggregate, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiAdd(aggregate, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAdd")]
		public static extern void x86_sdaiAdd(Int32 aggregate, Int32 valueType, string value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiAdd")]
		public static extern void x64_sdaiAdd(Int64 aggregate, Int64 valueType, string value);

		public static void sdaiAdd(Int64 aggregate, Int64 valueType, string value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiAdd((Int32)aggregate, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiAdd(aggregate, valueType, value);
			}
		}

		/// <summary>
		///		sdaiInsertByIndex                                       (https://rdf.bg/ifcdoc/CS64/sdaiInsertByIndex.html)
		///
		///	valueType argument to specify what type of data caller wants to put
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiInsertByIndex, and it works similarly for all put-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiInsertByIndex but valid for all put-functions)
		///
		///	valueType				C/C++															C#
		///
		///	sdaiINTEGER				int_t val = 123;												int_t val = 123;
		///							sdaiInsertByIndex (aggregate, index, sdaiINTEGER, &val);		ifcengine.sdaiInsertByIndex (aggregate, index, ifcengine.sdaiINTEGER, ref val);
		///
		///	sdaiREAL or sdaiNUMBER	double val = 123.456;											double val = 123.456;
		///							sdaiInsertByIndex (aggregate, index, sdaiREAL, &val);			ifcengine.sdaiInsertByIndex (aggregate, index, ifcengine.sdaiREAL, ref val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val = sdaiTRUE;										bool val = true;
		///							sdaiInsertByIndex (aggregate, index, sdaiBOOLEAN, &val);		ifcengine.sdaiInsertByIndex (aggregate, index, ifcengine.sdaiBOOLEAN, ref val);
		///
		///	sdaiLOGICAL				const TCHAR* val = "U";											string val = "U";
		///							sdaiInsertByIndex (aggregate, index, sdaiLOGICAL, val);			ifcengine.sdaiInsertByIndex (aggregate, index, ifcengine.sdaiLOGICAL, val);
		///
		///	sdaiENUM				const TCHAR* val = "NOTDEFINED";								string val = "NOTDEFINED";
		///							sdaiInsertByIndex (aggregate, index, sdaiENUM, val);			ifcengine.sdaiInsertByIndex (aggregate, index, ifcengine.sdaiENUM, val);
		///
		///	sdaiBINARY				const TCHAR* val = "0123456ABC";								string val = "0123456ABC";
		///							sdaiInsertByIndex (aggregate, index, sdaiBINARY, val);			ifcengine.sdaiInsertByIndex (aggregate, index, ifcengine.sdaiBINARY, val);
		///
		///	sdaiSTRING				const char* val = "My Simple String";							string val = "My Simple String";
		///							sdaiInsertByIndex (aggregate, index, sdaiSTRING, val);			ifcengine.sdaiInsertByIndex (aggregate, index, ifcengine.sdaiSTRING, val);
		///
		///	sdaiUNICODE				const wchar_t* val = L"Any Unicode String";						string val = "Any Unicode String";
		///							sdaiInsertByIndex (aggregate, index, sdaiUNICODE, val);			ifcengine.sdaiInsertByIndex (aggregate, index, ifcengine.sdaiUNICODE, val);
		///
		///	sdaiEXPRESSSTRING		const char* val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";		string val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";
		///							sdaiInsertByIndex (aggregate, index, sdaiEXPRESSSTRING, val);	ifcengine.sdaiInsertByIndex (aggregate, index, ifcengine.sdaiEXPRESSSTRING, val);
		///
		///	sdaiINSTANCE			SdaiInstance val = sdaiCreateInstanceBN (model, "IFCSITE");		int_t val = ifcengine.sdaiCreateInstanceBN (model, "IFCSITE");
		///							sdaiInsertByIndex (aggregate, index, sdaiINSTANCE, val);		ifcengine.sdaiInsertByIndex (aggregate, index, ifcengine.sdaiINSTANCE, val);
		///
		///	sdaiAGGR				SdaiAggr val = sdaiCreateAggr (inst, 0);						int_t val = sdaiCreateAggr (inst, 0);
		///							sdaiPutAttr (val, sdaiINSTANCE, inst);							ifcengine.sdaiPutAttr (val, ifcengine.sdaiINSTANCE, inst);
		///							sdaiInsertByIndex (aggregate, index, sdaiAGGR, val);			ifcengine.sdaiInsertByIndex (aggregate, index, ifcengine.sdaiAGGR, val);
		///
		///	sdaiADB					int_t integerValue = 123;										int_t integerValue = 123;	
		///							SdaiADB val = sdaiCreateADB (sdaiINTEGER, &integerValue);		int_t val = ifcengine.sdaiCreateADB (ifcengine.sdaiINTEGER, ref integerValue);
		///							sdaiPutADBTypePath (val, 1, "IFCINTEGER");						ifcengine.sdaiPutADBTypePath (val, 1, "IFCINTEGER");
		///							sdaiInsertByIndex (aggregate, index, sdaiADB, val);				ifcengine.sdaiInsertByIndex (aggregate, index, ifcengine.sdaiADB, val);	
		///							sdaiDeleteADB (val);											ifcengine.sdaiDeleteADB (val);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///	(Non-standard extension) sdiADB in C++ has an option to work without sdaiCreateEmptyADB and sdaiDeleteADB as shown in the table.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			 .			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			 .			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			 . 			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiUNICODE			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertByIndex")]
		public static extern void x86_sdaiInsertByIndex(Int32 aggregate, Int32 index, Int32 valueType, ref bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertByIndex")]
		public static extern void x64_sdaiInsertByIndex(Int64 aggregate, Int64 index, Int64 valueType, ref bool value);

		public static void sdaiInsertByIndex(Int64 aggregate, Int64 index, Int64 valueType, ref bool value)
		{
			if (IntPtr.Size == 4)
			{
				bool _value = (bool)value;
				x86_sdaiInsertByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiInsertByIndex(aggregate, index, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertByIndex")]
		public static extern void x86_sdaiInsertByIndex(Int32 aggregate, Int32 index, Int32 valueType, ref Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertByIndex")]
		public static extern void x64_sdaiInsertByIndex(Int64 aggregate, Int64 index, Int64 valueType, ref Int64 value);

		public static void sdaiInsertByIndex(Int64 aggregate, Int64 index, Int64 valueType, ref Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				Int32 _value = (Int32)value;
				x86_sdaiInsertByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiInsertByIndex(aggregate, index, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertByIndex")]
		public static extern void x86_sdaiInsertByIndex(Int32 aggregate, Int32 index, Int32 valueType, Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertByIndex")]
		public static extern void x64_sdaiInsertByIndex(Int64 aggregate, Int64 index, Int64 valueType, Int64 value);

		public static void sdaiInsertByIndex(Int64 aggregate, Int64 index, Int64 valueType, Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiInsertByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, (Int32)value);
			}
			else
			{
				x64_sdaiInsertByIndex(aggregate, index, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertByIndex")]
		public static extern void x86_sdaiInsertByIndex(Int32 aggregate, Int32 index, Int32 valueType, ref double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertByIndex")]
		public static extern void x64_sdaiInsertByIndex(Int64 aggregate, Int64 index, Int64 valueType, ref double value);

		public static void sdaiInsertByIndex(Int64 aggregate, Int64 index, Int64 valueType, ref double value)
		{
			if (IntPtr.Size == 4)
			{
				double _value = (double)value;
				x86_sdaiInsertByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiInsertByIndex(aggregate, index, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertByIndex")]
		public static extern void x86_sdaiInsertByIndex(Int32 aggregate, Int32 index, Int32 valueType, ref IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertByIndex")]
		public static extern void x64_sdaiInsertByIndex(Int64 aggregate, Int64 index, Int64 valueType, ref IntPtr value);

		public static void sdaiInsertByIndex(Int64 aggregate, Int64 index, Int64 valueType, ref IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				IntPtr _value = (IntPtr)value;
				x86_sdaiInsertByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiInsertByIndex(aggregate, index, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertByIndex")]
		public static extern void x86_sdaiInsertByIndex(Int32 aggregate, Int32 index, Int32 valueType, byte[] value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertByIndex")]
		public static extern void x64_sdaiInsertByIndex(Int64 aggregate, Int64 index, Int64 valueType, byte[] value);

		public static void sdaiInsertByIndex(Int64 aggregate, Int64 index, Int64 valueType, byte[] value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiInsertByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiInsertByIndex(aggregate, index, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertByIndex")]
		public static extern void x86_sdaiInsertByIndex(Int32 aggregate, Int32 index, Int32 valueType, string value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertByIndex")]
		public static extern void x64_sdaiInsertByIndex(Int64 aggregate, Int64 index, Int64 valueType, string value);

		public static void sdaiInsertByIndex(Int64 aggregate, Int64 index, Int64 valueType, string value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiInsertByIndex((Int32)aggregate, (Int32)index, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiInsertByIndex(aggregate, index, valueType, value);
			}
		}

		/// <summary>
		///		sdaiInsertBefore                                        (https://rdf.bg/ifcdoc/CS64/sdaiInsertBefore.html)
		///
		///	valueType argument to specify what type of data caller wants to put
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiInsertBefore, and it works similarly for all put-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiInsertBefore but valid for all put-functions)
		///
		///	valueType				C/C++														C#
		///
		///	sdaiINTEGER				int_t val = 123;											int_t val = 123;
		///							sdaiInsertBefore (iterator, sdaiINTEGER, &val);				ifcengine.sdaiInsertBefore (iterator, ifcengine.sdaiINTEGER, ref val);
		///
		///	sdaiREAL or sdaiNUMBER	double val = 123.456;										double val = 123.456;
		///							sdaiInsertBefore (iterator, sdaiREAL, &val);				ifcengine.sdaiInsertBefore (iterator, ifcengine.sdaiREAL, ref val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val = sdaiTRUE;									bool val = true;
		///							sdaiInsertBefore (iterator, sdaiBOOLEAN, &val);				ifcengine.sdaiInsertBefore (iterator, ifcengine.sdaiBOOLEAN, ref val);
		///
		///	sdaiLOGICAL				const TCHAR* val = "U";										string val = "U";
		///							sdaiInsertBefore (iterator, sdaiLOGICAL, val);				ifcengine.sdaiInsertBefore (iterator, ifcengine.sdaiLOGICAL, val);
		///
		///	sdaiENUM				const TCHAR* val = "NOTDEFINED";							string val = "NOTDEFINED";
		///							sdaiInsertBefore (iterator, sdaiENUM, val);					ifcengine.sdaiInsertBefore (iterator, ifcengine.sdaiENUM, val);
		///
		///	sdaiBINARY				const TCHAR* val = "0123456ABC";							string val = "0123456ABC";
		///							sdaiInsertBefore (iterator, sdaiBINARY, val);				ifcengine.sdaiInsertBefore (iterator, ifcengine.sdaiBINARY, val);
		///
		///	sdaiSTRING				const char* val = "My Simple String";						string val = "My Simple String";
		///							sdaiInsertBefore (iterator, sdaiSTRING, val);				ifcengine.sdaiInsertBefore (iterator, ifcengine.sdaiSTRING, val);
		///
		///	sdaiUNICODE				const wchar_t* val = L"Any Unicode String";					string val = "Any Unicode String";
		///							sdaiInsertBefore (iterator, sdaiUNICODE, val);				ifcengine.sdaiInsertBefore (iterator, ifcengine.sdaiUNICODE, val);
		///
		///	sdaiEXPRESSSTRING		const char* val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";	string val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";
		///							sdaiInsertBefore (iterator, sdaiEXPRESSSTRING, val);		ifcengine.sdaiInsertBefore (iterator, ifcengine.sdaiEXPRESSSTRING, val);
		///
		///	sdaiINSTANCE			SdaiInstance val = sdaiCreateInstanceBN (model, "IFCSITE");	int_t val = ifcengine.sdaiCreateInstanceBN (model, "IFCSITE");
		///							sdaiInsertBefore (iterator, sdaiINSTANCE, val);				ifcengine.sdaiInsertBefore (iterator, ifcengine.sdaiINSTANCE, val);
		///
		///	sdaiAGGR				SdaiAggr val = sdaiCreateAggr (inst, 0);					int_t val = sdaiCreateAggr (inst, 0);
		///							sdaiPutAttr (val, sdaiINSTANCE, inst);						ifcengine.sdaiPutAttr (val, ifcengine.sdaiINSTANCE, inst);
		///							sdaiInsertBefore (iterator, sdaiAGGR, val);					ifcengine.sdaiInsertBefore (iterator, ifcengine.sdaiAGGR, val);
		///
		///	sdaiADB					int_t integerValue = 123;									int_t integerValue = 123;	
		///							SdaiADB val = sdaiCreateADB (sdaiINTEGER, &integerValue);	int_t val = ifcengine.sdaiCreateADB (ifcengine.sdaiINTEGER, ref integerValue);
		///							sdaiPutADBTypePath (val, 1, "IFCINTEGER");					ifcengine.sdaiPutADBTypePath (val, 1, "IFCINTEGER");
		///							sdaiInsertBefore (iterator, sdaiADB, val);					ifcengine.sdaiInsertBefore (iterator, ifcengine.sdaiADB, val);	
		///							sdaiDeleteADB (val);										ifcengine.sdaiDeleteADB (val);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///	(Non-standard extension) sdiADB in C++ has an option to work without sdaiCreateEmptyADB and sdaiDeleteADB as shown in the table.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			 .			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			 .			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			 . 			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiUNICODE			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertBefore")]
		public static extern void x86_sdaiInsertBefore(Int32 iterator, Int32 valueType, ref bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertBefore")]
		public static extern void x64_sdaiInsertBefore(Int64 iterator, Int64 valueType, ref bool value);

		public static void sdaiInsertBefore(Int64 iterator, Int64 valueType, ref bool value)
		{
			if (IntPtr.Size == 4)
			{
				bool _value = (bool)value;
				x86_sdaiInsertBefore((Int32)iterator, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiInsertBefore(iterator, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertBefore")]
		public static extern void x86_sdaiInsertBefore(Int32 iterator, Int32 valueType, ref Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertBefore")]
		public static extern void x64_sdaiInsertBefore(Int64 iterator, Int64 valueType, ref Int64 value);

		public static void sdaiInsertBefore(Int64 iterator, Int64 valueType, ref Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				Int32 _value = (Int32)value;
				x86_sdaiInsertBefore((Int32)iterator, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiInsertBefore(iterator, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertBefore")]
		public static extern void x86_sdaiInsertBefore(Int32 iterator, Int32 valueType, Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertBefore")]
		public static extern void x64_sdaiInsertBefore(Int64 iterator, Int64 valueType, Int64 value);

		public static void sdaiInsertBefore(Int64 iterator, Int64 valueType, Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiInsertBefore((Int32)iterator, (Int32)valueType, (Int32)value);
			}
			else
			{
				x64_sdaiInsertBefore(iterator, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertBefore")]
		public static extern void x86_sdaiInsertBefore(Int32 iterator, Int32 valueType, ref double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertBefore")]
		public static extern void x64_sdaiInsertBefore(Int64 iterator, Int64 valueType, ref double value);

		public static void sdaiInsertBefore(Int64 iterator, Int64 valueType, ref double value)
		{
			if (IntPtr.Size == 4)
			{
				double _value = (double)value;
				x86_sdaiInsertBefore((Int32)iterator, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiInsertBefore(iterator, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertBefore")]
		public static extern void x86_sdaiInsertBefore(Int32 iterator, Int32 valueType, ref IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertBefore")]
		public static extern void x64_sdaiInsertBefore(Int64 iterator, Int64 valueType, ref IntPtr value);

		public static void sdaiInsertBefore(Int64 iterator, Int64 valueType, ref IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				IntPtr _value = (IntPtr)value;
				x86_sdaiInsertBefore((Int32)iterator, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiInsertBefore(iterator, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertBefore")]
		public static extern void x86_sdaiInsertBefore(Int32 iterator, Int32 valueType, byte[] value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertBefore")]
		public static extern void x64_sdaiInsertBefore(Int64 iterator, Int64 valueType, byte[] value);

		public static void sdaiInsertBefore(Int64 iterator, Int64 valueType, byte[] value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiInsertBefore((Int32)iterator, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiInsertBefore(iterator, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertBefore")]
		public static extern void x86_sdaiInsertBefore(Int32 iterator, Int32 valueType, string value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertBefore")]
		public static extern void x64_sdaiInsertBefore(Int64 iterator, Int64 valueType, string value);

		public static void sdaiInsertBefore(Int64 iterator, Int64 valueType, string value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiInsertBefore((Int32)iterator, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiInsertBefore(iterator, valueType, value);
			}
		}

		/// <summary>
		///		sdaiInsertAfter                                         (https://rdf.bg/ifcdoc/CS64/sdaiInsertAfter.html)
		///
		///	valueType argument to specify what type of data caller wants to put
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiInsertAfter, and it works similarly for all put-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiInsertAfter but valid for all put-functions)
		///
		///	valueType				C/C++														C#
		///
		///	sdaiINTEGER				int_t val = 123;											int_t val = 123;
		///							sdaiInsertAfter (iterator, sdaiINTEGER, &val);				ifcengine.sdaiInsertAfter (iterator, ifcengine.sdaiINTEGER, ref val);
		///
		///	sdaiREAL or sdaiNUMBER	double val = 123.456;										double val = 123.456;
		///							sdaiInsertAfter (iterator, sdaiREAL, &val);					ifcengine.sdaiInsertAfter (iterator, ifcengine.sdaiREAL, ref val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val = sdaiTRUE;									bool val = true;
		///							sdaiInsertAfter (iterator, sdaiBOOLEAN, &val);				ifcengine.sdaiInsertAfter (iterator, ifcengine.sdaiBOOLEAN, ref val);
		///
		///	sdaiLOGICAL				const TCHAR* val = "U";										string val = "U";
		///							sdaiInsertAfter (iterator, sdaiLOGICAL, val);				ifcengine.sdaiInsertAfter (iterator, ifcengine.sdaiLOGICAL, val);
		///
		///	sdaiENUM				const TCHAR* val = "NOTDEFINED";							string val = "NOTDEFINED";
		///							sdaiInsertAfter (iterator, sdaiENUM, val);					ifcengine.sdaiInsertAfter (iterator, ifcengine.sdaiENUM, val);
		///
		///	sdaiBINARY				const TCHAR* val = "0123456ABC";							string val = "0123456ABC";
		///							sdaiInsertAfter (iterator, sdaiBINARY, val);				ifcengine.sdaiInsertAfter (iterator, ifcengine.sdaiBINARY, val);
		///
		///	sdaiSTRING				const char* val = "My Simple String";						string val = "My Simple String";
		///							sdaiInsertAfter (iterator, sdaiSTRING, val);				ifcengine.sdaiInsertAfter (iterator, ifcengine.sdaiSTRING, val);
		///
		///	sdaiUNICODE				const wchar_t* val = L"Any Unicode String";					string val = "Any Unicode String";
		///							sdaiInsertAfter (iterator, sdaiUNICODE, val);				ifcengine.sdaiInsertAfter (iterator, ifcengine.sdaiUNICODE, val);
		///
		///	sdaiEXPRESSSTRING		const char* val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";	string val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";
		///							sdaiInsertAfter (iterator, sdaiEXPRESSSTRING, val);			ifcengine.sdaiInsertAfter (iterator, ifcengine.sdaiEXPRESSSTRING, val);
		///
		///	sdaiINSTANCE			SdaiInstance val = sdaiCreateInstanceBN (model, "IFCSITE");	int_t val = ifcengine.sdaiCreateInstanceBN (model, "IFCSITE");
		///							sdaiInsertAfter (iterator, sdaiINSTANCE, val);				ifcengine.sdaiInsertAfter (iterator, ifcengine.sdaiINSTANCE, val);
		///
		///	sdaiAGGR				SdaiAggr val = sdaiCreateAggr (inst, 0);					int_t val = sdaiCreateAggr (inst, 0);
		///							sdaiPutAttr (val, sdaiINSTANCE, inst);						ifcengine.sdaiPutAttr (val, ifcengine.sdaiINSTANCE, inst);
		///							sdaiInsertAfter (iterator, sdaiAGGR, val);					ifcengine.sdaiInsertAfter (iterator, ifcengine.sdaiAGGR, val);
		///
		///	sdaiADB					int_t integerValue = 123;									int_t integerValue = 123;	
		///							SdaiADB val = sdaiCreateADB (sdaiINTEGER, &integerValue);	int_t val = ifcengine.sdaiCreateADB (ifcengine.sdaiINTEGER, ref integerValue);
		///							sdaiPutADBTypePath (val, 1, "IFCINTEGER");					ifcengine.sdaiPutADBTypePath (val, 1, "IFCINTEGER");
		///							sdaiInsertAfter (iterator, sdaiADB, val);					ifcengine.sdaiInsertAfter (iterator, ifcengine.sdaiADB, val);	
		///							sdaiDeleteADB (val);										ifcengine.sdaiDeleteADB (val);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///	(Non-standard extension) sdiADB in C++ has an option to work without sdaiCreateEmptyADB and sdaiDeleteADB as shown in the table.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			 .			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			 .			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			 . 			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiUNICODE			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertAfter")]
		public static extern void x86_sdaiInsertAfter(Int32 iterator, Int32 valueType, ref bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertAfter")]
		public static extern void x64_sdaiInsertAfter(Int64 iterator, Int64 valueType, ref bool value);

		public static void sdaiInsertAfter(Int64 iterator, Int64 valueType, ref bool value)
		{
			if (IntPtr.Size == 4)
			{
				bool _value = (bool)value;
				x86_sdaiInsertAfter((Int32)iterator, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiInsertAfter(iterator, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertAfter")]
		public static extern void x86_sdaiInsertAfter(Int32 iterator, Int32 valueType, ref Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertAfter")]
		public static extern void x64_sdaiInsertAfter(Int64 iterator, Int64 valueType, ref Int64 value);

		public static void sdaiInsertAfter(Int64 iterator, Int64 valueType, ref Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				Int32 _value = (Int32)value;
				x86_sdaiInsertAfter((Int32)iterator, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiInsertAfter(iterator, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertAfter")]
		public static extern void x86_sdaiInsertAfter(Int32 iterator, Int32 valueType, Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertAfter")]
		public static extern void x64_sdaiInsertAfter(Int64 iterator, Int64 valueType, Int64 value);

		public static void sdaiInsertAfter(Int64 iterator, Int64 valueType, Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiInsertAfter((Int32)iterator, (Int32)valueType, (Int32)value);
			}
			else
			{
				x64_sdaiInsertAfter(iterator, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertAfter")]
		public static extern void x86_sdaiInsertAfter(Int32 iterator, Int32 valueType, ref double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertAfter")]
		public static extern void x64_sdaiInsertAfter(Int64 iterator, Int64 valueType, ref double value);

		public static void sdaiInsertAfter(Int64 iterator, Int64 valueType, ref double value)
		{
			if (IntPtr.Size == 4)
			{
				double _value = (double)value;
				x86_sdaiInsertAfter((Int32)iterator, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiInsertAfter(iterator, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertAfter")]
		public static extern void x86_sdaiInsertAfter(Int32 iterator, Int32 valueType, ref IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertAfter")]
		public static extern void x64_sdaiInsertAfter(Int64 iterator, Int64 valueType, ref IntPtr value);

		public static void sdaiInsertAfter(Int64 iterator, Int64 valueType, ref IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				IntPtr _value = (IntPtr)value;
				x86_sdaiInsertAfter((Int32)iterator, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiInsertAfter(iterator, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertAfter")]
		public static extern void x86_sdaiInsertAfter(Int32 iterator, Int32 valueType, byte[] value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertAfter")]
		public static extern void x64_sdaiInsertAfter(Int64 iterator, Int64 valueType, byte[] value);

		public static void sdaiInsertAfter(Int64 iterator, Int64 valueType, byte[] value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiInsertAfter((Int32)iterator, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiInsertAfter(iterator, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertAfter")]
		public static extern void x86_sdaiInsertAfter(Int32 iterator, Int32 valueType, string value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertAfter")]
		public static extern void x64_sdaiInsertAfter(Int64 iterator, Int64 valueType, string value);

		public static void sdaiInsertAfter(Int64 iterator, Int64 valueType, string value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiInsertAfter((Int32)iterator, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiInsertAfter(iterator, valueType, value);
			}
		}

		/// <summary>
		///		sdaiCreateADB                                           (https://rdf.bg/ifcdoc/CS64/sdaiCreateADB.html)
		///
		///	valueType argument to specify what type of data caller wants to put
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiCreateADB, and it works similarly for all put-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiCreateADB but valid for all put-functions)
		///
		///	valueType				C/C++														C#
		///
		///	sdaiINTEGER				int_t val = 123;											int_t val = 123;
		///							SdaiADB adb = sdaiCreateADB (sdaiINTEGER, &val);			int_t adb = ifcengine.sdaiCreateADB (ifcengine.sdaiINTEGER, ref val);
		///
		///	sdaiREAL or sdaiNUMBER	double val = 123.456;										double val = 123.456;
		///							SdaiADB adb = sdaiCreateADB (sdaiREAL, &val);				int_t adb = ifcengine.sdaiCreateADB (ifcengine.sdaiREAL, ref val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val = sdaiTRUE;									bool val = true;
		///							SdaiADB adb = sdaiCreateADB (sdaiBOOLEAN, &val);			int_t adb = ifcengine.sdaiCreateADB (ifcengine.sdaiBOOLEAN, ref val);
		///
		///	sdaiLOGICAL				const TCHAR* val = "U";										string val = "U";
		///							SdaiADB adb = sdaiCreateADB (sdaiLOGICAL, val);				int_t adb = ifcengine.sdaiCreateADB (ifcengine.sdaiLOGICAL, val);
		///
		///	sdaiENUM				const TCHAR* val = "NOTDEFINED";							string val = "NOTDEFINED";
		///							SdaiADB adb = sdaiCreateADB (sdaiENUM, val);				int_t adb = ifcengine.sdaiCreateADB (ifcengine.sdaiENUM, val);
		///
		///	sdaiBINARY				const TCHAR* val = "0123456ABC";							string val = "0123456ABC";
		///							SdaiADB adb = sdaiCreateADB (sdaiBINARY, val);				int_t adb = ifcengine.sdaiCreateADB (ifcengine.sdaiBINARY, val);
		///
		///	sdaiSTRING				const char* val = "My Simple String";						string val = "My Simple String";
		///							SdaiADB adb = sdaiCreateADB (sdaiSTRING, val);				int_t adb = ifcengine.sdaiCreateADB (ifcengine.sdaiSTRING, val);
		///
		///	sdaiUNICODE				const wchar_t* val = L"Any Unicode String";					string val = "Any Unicode String";
		///							SdaiADB adb = sdaiCreateADB (sdaiUNICODE, val);				int_t adb = ifcengine.sdaiCreateADB (ifcengine.sdaiUNICODE, val);
		///
		///	sdaiEXPRESSSTRING		const char* val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";	string val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";
		///							SdaiADB adb = sdaiCreateADB (sdaiEXPRESSSTRING, val);		int_t adb = ifcengine.sdaiCreateADB (ifcengine.sdaiEXPRESSSTRING, val);
		///
		///	sdaiINSTANCE			SdaiInstance val = sdaiCreateInstanceBN (model, "IFCSITE");	int_t val = ifcengine.sdaiCreateInstanceBN (model, "IFCSITE");
		///							SdaiADB adb = sdaiCreateADB (sdaiINSTANCE, val);			int_t adb = ifcengine.sdaiCreateADB (ifcengine.sdaiINSTANCE, val);
		///
		///	sdaiAGGR				SdaiAggr val = sdaiCreateAggr (inst, 0);					int_t val = sdaiCreateAggr (inst, 0);
		///							sdaiPutAttr (val, sdaiINSTANCE, inst);						ifcengine.sdaiPutAttr (val, ifcengine.sdaiINSTANCE, inst);
		///							SdaiADB adb = sdaiCreateADB (sdaiAGGR, val);				int_t adb = ifcengine.sdaiCreateADB (ifcengine.sdaiAGGR, val);
		///
		///	sdaiADB					not applicable
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			 .			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			 .			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			 . 			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiUNICODE			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateADB")]
		public static extern Int32 x86_sdaiCreateADB(Int32 valueType, ref bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateADB")]
		public static extern Int64 x64_sdaiCreateADB(Int64 valueType, ref bool value);

		public static Int64 sdaiCreateADB(Int64 valueType, ref bool value)
		{
			if (IntPtr.Size == 4)
			{
				bool _value = (bool)value;
				var _result = x86_sdaiCreateADB((Int32)valueType, ref _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiCreateADB(valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateADB")]
		public static extern Int32 x86_sdaiCreateADB(Int32 valueType, ref Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateADB")]
		public static extern Int64 x64_sdaiCreateADB(Int64 valueType, ref Int64 value);

		public static Int64 sdaiCreateADB(Int64 valueType, ref Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				Int32 _value = (Int32)value;
				var _result = x86_sdaiCreateADB((Int32)valueType, ref _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiCreateADB(valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateADB")]
		public static extern Int32 x86_sdaiCreateADB(Int32 valueType, Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateADB")]
		public static extern Int64 x64_sdaiCreateADB(Int64 valueType, Int64 value);

		public static Int64 sdaiCreateADB(Int64 valueType, Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateADB((Int32)valueType, (Int32)value);
				return _result;
			}
			else
			{
				return x64_sdaiCreateADB(valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateADB")]
		public static extern Int32 x86_sdaiCreateADB(Int32 valueType, ref double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateADB")]
		public static extern Int64 x64_sdaiCreateADB(Int64 valueType, ref double value);

		public static Int64 sdaiCreateADB(Int64 valueType, ref double value)
		{
			if (IntPtr.Size == 4)
			{
				double _value = (double)value;
				var _result = x86_sdaiCreateADB((Int32)valueType, ref _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiCreateADB(valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateADB")]
		public static extern Int32 x86_sdaiCreateADB(Int32 valueType, ref IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateADB")]
		public static extern Int64 x64_sdaiCreateADB(Int64 valueType, ref IntPtr value);

		public static Int64 sdaiCreateADB(Int64 valueType, ref IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				IntPtr _value = (IntPtr)value;
				var _result = x86_sdaiCreateADB((Int32)valueType, ref _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiCreateADB(valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateADB")]
		public static extern Int32 x86_sdaiCreateADB(Int32 valueType, byte[] value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateADB")]
		public static extern Int64 x64_sdaiCreateADB(Int64 valueType, byte[] value);

		public static Int64 sdaiCreateADB(Int64 valueType, byte[] value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateADB((Int32)valueType, value);
				return _result;
			}
			else
			{
				return x64_sdaiCreateADB(valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateADB")]
		public static extern Int32 x86_sdaiCreateADB(Int32 valueType, string value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateADB")]
		public static extern Int64 x64_sdaiCreateADB(Int64 valueType, string value);

		public static Int64 sdaiCreateADB(Int64 valueType, string value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateADB((Int32)valueType, value);
				return _result;
			}
			else
			{
				return x64_sdaiCreateADB(valueType, value);
			}
		}

		/// <summary>
		///		sdaiCreateAggr                                          (https://rdf.bg/ifcdoc/CS64/sdaiCreateAggr.html)
		///
		///	This call creates an aggregation.
		///	The instance has to be present,
		///	the attribute argument can be empty (0) in case the aggregation is an nested aggregation for this specific instance,
		///	preferred use would be use of sdaiCreateNestedAggr in such a case.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateAggr")]
		public static extern Int32 x86_sdaiCreateAggr(Int32 instance, Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateAggr")]
		public static extern Int64 x64_sdaiCreateAggr(Int64 instance, Int64 attribute);

		public static Int64 sdaiCreateAggr(Int64 instance, Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateAggr((Int32)instance, (Int32)attribute);
				return _result;
			}
			else
			{
				return x64_sdaiCreateAggr(instance, attribute);
			}
		}

		/// <summary>
		///		sdaiCreateAggrBN                                        (https://rdf.bg/ifcdoc/CS64/sdaiCreateAggrBN.html)
		///
		///	This call creates an aggregation.
		///	The instance has to be present,
		///	the attributeName argument can be NULL (0) in case the aggregation is an nested aggregation for this specific instance,
		///	preferred use would be use of sdaiCreateNestedAggr in such a case.
		///
		///	Technically sdaiCreateAggrBN will transform into the following call
		///		(attributeName) ?
		///			sdaiCreateAggr(
		///					instance,
		///					sdaiGetAttrDefinition(
		///							sdaiGetInstanceType(
		///									instance
		///								),
		///							attributeName
		///						)
		///				) :
		///			sdaiCreateAggr(
		///					instance,
		///					nullptr
		///				);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateAggrBN")]
		public static extern Int32 x86_sdaiCreateAggrBN(Int32 instance, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateAggrBN")]
		public static extern Int64 x64_sdaiCreateAggrBN(Int64 instance, string attributeName);

		public static Int64 sdaiCreateAggrBN(Int64 instance, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateAggrBN((Int32)instance, attributeName);
				return _result;
			}
			else
			{
				return x64_sdaiCreateAggrBN(instance, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateAggrBN")]
		public static extern Int32 x86_sdaiCreateAggrBN(Int32 instance, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateAggrBN")]
		public static extern Int64 x64_sdaiCreateAggrBN(Int64 instance, byte[] attributeName);

		public static Int64 sdaiCreateAggrBN(Int64 instance, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateAggrBN((Int32)instance, attributeName);
				return _result;
			}
			else
			{
				return x64_sdaiCreateAggrBN(instance, attributeName);
			}
		}

		/// <summary>
		///		sdaiCreateNPL                                           (https://rdf.bg/ifcdoc/CS64/sdaiCreateNPL.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateNPL")]
		public static extern Int32 x86_sdaiCreateNPL();

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateNPL")]
		public static extern Int64 x64_sdaiCreateNPL();

		public static Int64 sdaiCreateNPL()
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateNPL();
				return _result;
			}
			else
			{
				return x64_sdaiCreateNPL();
			}
		}

		/// <summary>
		///		sdaiDeleteNPL                                           (https://rdf.bg/ifcdoc/CS64/sdaiDeleteNPL.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiDeleteNPL")]
		public static extern void x86_sdaiDeleteNPL(Int32 list);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiDeleteNPL")]
		public static extern void x64_sdaiDeleteNPL(Int64 list);

		public static void sdaiDeleteNPL(Int64 list)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiDeleteNPL((Int32)list);
			}
			else
			{
				x64_sdaiDeleteNPL(list);
			}
		}

		/// <summary>
		///		sdaiCreateNestedAggr                                    (https://rdf.bg/ifcdoc/CS64/sdaiCreateNestedAggr.html)
		///
		///	This call creates an aggregation within an aggregation.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateNestedAggr")]
		public static extern Int32 x86_sdaiCreateNestedAggr(Int32 aggregate);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateNestedAggr")]
		public static extern Int64 x64_sdaiCreateNestedAggr(Int64 aggregate);

		public static Int64 sdaiCreateNestedAggr(Int64 aggregate)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateNestedAggr((Int32)aggregate);
				return _result;
			}
			else
			{
				return x64_sdaiCreateNestedAggr(aggregate);
			}
		}

		/// <summary>
		///		sdaiCreateNestedAggrByIndex                             (https://rdf.bg/ifcdoc/CS64/sdaiCreateNestedAggrByIndex.html)
		///
		///	The function creates an aggregate instance and replaces the existing member of the specified ordered aggregate instance
		///	referenced by the specified index.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateNestedAggrByIndex")]
		public static extern Int32 x86_sdaiCreateNestedAggrByIndex(Int32 aggregate, Int32 index);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateNestedAggrByIndex")]
		public static extern Int64 x64_sdaiCreateNestedAggrByIndex(Int64 aggregate, Int64 index);

		public static Int64 sdaiCreateNestedAggrByIndex(Int64 aggregate, Int64 index)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateNestedAggrByIndex((Int32)aggregate, (Int32)index);
				return _result;
			}
			else
			{
				return x64_sdaiCreateNestedAggrByIndex(aggregate, index);
			}
		}

		/// <summary>
		///		sdaiInsertNestedAggrByIndex                             (https://rdf.bg/ifcdoc/CS64/sdaiInsertNestedAggrByIndex.html)
		///
		///	The function creates an aggregate instance as a member of the specified ordered aggregate instance.
		///	The newly created aggregate is inserted into the aggregate at the position referenced by the specified index.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertNestedAggrByIndex")]
		public static extern Int32 x86_sdaiInsertNestedAggrByIndex(Int32 aggregate, Int32 index);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertNestedAggrByIndex")]
		public static extern Int64 x64_sdaiInsertNestedAggrByIndex(Int64 aggregate, Int64 index);

		public static Int64 sdaiInsertNestedAggrByIndex(Int64 aggregate, Int64 index)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiInsertNestedAggrByIndex((Int32)aggregate, (Int32)index);
				return _result;
			}
			else
			{
				return x64_sdaiInsertNestedAggrByIndex(aggregate, index);
			}
		}

		/// <summary>
		///		sdaiCreateNestedAggrByItr                               (https://rdf.bg/ifcdoc/CS64/sdaiCreateNestedAggrByItr.html)
		///
		///	The function creates an aggregate instance replacing the current member of the aggregate instance
		///	referenced by the specified iterator.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateNestedAggrByItr")]
		public static extern Int32 x86_sdaiCreateNestedAggrByItr(Int32 iterator);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateNestedAggrByItr")]
		public static extern Int64 x64_sdaiCreateNestedAggrByItr(Int64 iterator);

		public static Int64 sdaiCreateNestedAggrByItr(Int64 iterator)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateNestedAggrByItr((Int32)iterator);
				return _result;
			}
			else
			{
				return x64_sdaiCreateNestedAggrByItr(iterator);
			}
		}

		/// <summary>
		///		sdaiInsertNestedAggrBefore                              (https://rdf.bg/ifcdoc/CS64/sdaiInsertNestedAggrBefore.html)
		///
		///	The function creates an aggregate instance as a member of a list instance.
		///	The newly created aggregate is inserted into the list instance before the member referenced by the specified iterator.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertNestedAggrBefore")]
		public static extern Int32 x86_sdaiInsertNestedAggrBefore(Int32 iterator);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertNestedAggrBefore")]
		public static extern Int64 x64_sdaiInsertNestedAggrBefore(Int64 iterator);

		public static Int64 sdaiInsertNestedAggrBefore(Int64 iterator)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiInsertNestedAggrBefore((Int32)iterator);
				return _result;
			}
			else
			{
				return x64_sdaiInsertNestedAggrBefore(iterator);
			}
		}

		/// <summary>
		///		sdaiInsertNestedAggrAfter                               (https://rdf.bg/ifcdoc/CS64/sdaiInsertNestedAggrAfter.html)
		///
		///	The function creates an aggregate instance as a member of a list instance.
		///	The newly created aggregate is inserted into the list instance after the member referenced by the specified iterator.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertNestedAggrAfter")]
		public static extern Int32 x86_sdaiInsertNestedAggrAfter(Int32 iterator);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertNestedAggrAfter")]
		public static extern Int64 x64_sdaiInsertNestedAggrAfter(Int64 iterator);

		public static Int64 sdaiInsertNestedAggrAfter(Int64 iterator)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiInsertNestedAggrAfter((Int32)iterator);
				return _result;
			}
			else
			{
				return x64_sdaiInsertNestedAggrAfter(iterator);
			}
		}

		/// <summary>
		///		sdaiCreateNestedAggrADB                                 (https://rdf.bg/ifcdoc/CS64/sdaiCreateNestedAggrADB.html)
		///
		///	The CreateNestedAggrABD function creates an aggregate instance as a member of (an unordered)
		///	aggregate instance in the case where the type of the aggregate to create is a SELECT TYPE and
		///	ambiguous.
		///	Input ADB is expected to have type path.
		///	The function sets the value of the ADB with the identifier of the newly created aggregate instance.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateNestedAggrADB")]
		public static extern Int32 x86_sdaiCreateNestedAggrADB(Int32 aggregate, Int32 selaggrInstance);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateNestedAggrADB")]
		public static extern Int64 x64_sdaiCreateNestedAggrADB(Int64 aggregate, Int64 selaggrInstance);

		public static Int64 sdaiCreateNestedAggrADB(Int64 aggregate, Int64 selaggrInstance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateNestedAggrADB((Int32)aggregate, (Int32)selaggrInstance);
				return _result;
			}
			else
			{
				return x64_sdaiCreateNestedAggrADB(aggregate, selaggrInstance);
			}
		}

		/// <summary>
		///		sdaiCreateNestedAggrByIndexADB                          (https://rdf.bg/ifcdoc/CS64/sdaiCreateNestedAggrByIndexADB.html)
		///
		///	The function creates an aggregate instance and replaces the existing member of the specified ordered aggregate instance 
		///	referenced by the specified index.
		///	Input ADB is expected to have type path.
		///	The function sets the value of the ADB with the identifier of the newly created aggregate instance.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateNestedAggrByIndexADB")]
		public static extern Int32 x86_sdaiCreateNestedAggrByIndexADB(Int32 aggregate, Int32 index, Int32 selaggrInstance);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateNestedAggrByIndexADB")]
		public static extern Int64 x64_sdaiCreateNestedAggrByIndexADB(Int64 aggregate, Int64 index, Int64 selaggrInstance);

		public static Int64 sdaiCreateNestedAggrByIndexADB(Int64 aggregate, Int64 index, Int64 selaggrInstance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateNestedAggrByIndexADB((Int32)aggregate, (Int32)index, (Int32)selaggrInstance);
				return _result;
			}
			else
			{
				return x64_sdaiCreateNestedAggrByIndexADB(aggregate, index, selaggrInstance);
			}
		}

		/// <summary>
		///		sdaiInsertNestedAggrByIndexADB                          (https://rdf.bg/ifcdoc/CS64/sdaiInsertNestedAggrByIndexADB.html)
		///
		///	The function creates an aggregate instance as member of the specified ordered aggregate instance. 
		///	The newly created aggregate is inserted into the aggregate at the position referenced by the specified index.
		///	Input ADB is expected to have type path.
		///	The function sets the value of the ADB with the identifier of the newly created aggregate instance.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertNestedAggrByIndexADB")]
		public static extern Int32 x86_sdaiInsertNestedAggrByIndexADB(Int32 aggregate, Int32 index, Int32 selaggrInstance);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertNestedAggrByIndexADB")]
		public static extern Int64 x64_sdaiInsertNestedAggrByIndexADB(Int64 aggregate, Int64 index, Int64 selaggrInstance);

		public static Int64 sdaiInsertNestedAggrByIndexADB(Int64 aggregate, Int64 index, Int64 selaggrInstance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiInsertNestedAggrByIndexADB((Int32)aggregate, (Int32)index, (Int32)selaggrInstance);
				return _result;
			}
			else
			{
				return x64_sdaiInsertNestedAggrByIndexADB(aggregate, index, selaggrInstance);
			}
		}

		/// <summary>
		///		sdaiCreateNestedAggrByItrADB                            (https://rdf.bg/ifcdoc/CS64/sdaiCreateNestedAggrByItrADB.html)
		///
		///	The function creates an aggregate instance replacing the current member of the aggregate instance 
		///	referenced by the specified iterator where the type of the aggregate to create is a SELECT TYPE and ambiguous,
		///	Input ADB is expected to have type path.
		///	The function sets the value of the ADB with the identifier of the newly created aggregate instance.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateNestedAggrByItrADB")]
		public static extern Int32 x86_sdaiCreateNestedAggrByItrADB(Int32 iterator, Int32 selaggrInstance);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateNestedAggrByItrADB")]
		public static extern Int64 x64_sdaiCreateNestedAggrByItrADB(Int64 iterator, Int64 selaggrInstance);

		public static Int64 sdaiCreateNestedAggrByItrADB(Int64 iterator, Int64 selaggrInstance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateNestedAggrByItrADB((Int32)iterator, (Int32)selaggrInstance);
				return _result;
			}
			else
			{
				return x64_sdaiCreateNestedAggrByItrADB(iterator, selaggrInstance);
			}
		}

		/// <summary>
		///		sdaiInsertNestedAggrBeforeADB                           (https://rdf.bg/ifcdoc/CS64/sdaiInsertNestedAggrBeforeADB.html)
		///
		///	The function creates an aggregate instance as a member of a list instance where the type of the aggregate to create is a SELECT TYPE and ambiguous.
		///	The newly created aggregate is inserted into the list instance before the member referenced by the specified iterator.
		///	Input ADB is expected to have type path.
		///	The function sets the value of the ADB with the identifier of the newly created aggregate instance.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertNestedAggrBeforeADB")]
		public static extern Int32 x86_sdaiInsertNestedAggrBeforeADB(Int32 iterator, Int32 selaggrInstance);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertNestedAggrBeforeADB")]
		public static extern Int64 x64_sdaiInsertNestedAggrBeforeADB(Int64 iterator, Int64 selaggrInstance);

		public static Int64 sdaiInsertNestedAggrBeforeADB(Int64 iterator, Int64 selaggrInstance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiInsertNestedAggrBeforeADB((Int32)iterator, (Int32)selaggrInstance);
				return _result;
			}
			else
			{
				return x64_sdaiInsertNestedAggrBeforeADB(iterator, selaggrInstance);
			}
		}

		/// <summary>
		///		sdaiInsertNestedAggrAfterADB                            (https://rdf.bg/ifcdoc/CS64/sdaiInsertNestedAggrAfterADB.html)
		///
		///	The function creates an aggregate instance as a member of a list instance where the type of the aggregate to create is a SELECT TYPE and ambiguous.
		///	The newly created aggregate is inserted into the list instance after the member referenced by the specified iterator.
		///	Input ADB is expected to have type path.
		///	The function sets the value of the ADB with the identifier of the newly created aggregate instance.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertNestedAggrAfterADB")]
		public static extern Int32 x86_sdaiInsertNestedAggrAfterADB(Int32 iterator, Int32 selaggrInstance);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiInsertNestedAggrAfterADB")]
		public static extern Int64 x64_sdaiInsertNestedAggrAfterADB(Int64 iterator, Int64 selaggrInstance);

		public static Int64 sdaiInsertNestedAggrAfterADB(Int64 iterator, Int64 selaggrInstance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiInsertNestedAggrAfterADB((Int32)iterator, (Int32)selaggrInstance);
				return _result;
			}
			else
			{
				return x64_sdaiInsertNestedAggrAfterADB(iterator, selaggrInstance);
			}
		}

		/// <summary>
		///		sdaiRemoveByIndex                                       (https://rdf.bg/ifcdoc/CS64/sdaiRemoveByIndex.html)
		///
		///	The function removes the member of the specified list referenced by the specified index.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemoveByIndex")]
		public static extern void x86_sdaiRemoveByIndex(Int32 aggregate, Int32 index);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemoveByIndex")]
		public static extern void x64_sdaiRemoveByIndex(Int64 aggregate, Int64 index);

		public static void sdaiRemoveByIndex(Int64 aggregate, Int64 index)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiRemoveByIndex((Int32)aggregate, (Int32)index);
			}
			else
			{
				x64_sdaiRemoveByIndex(aggregate, index);
			}
		}

		/// <summary>
		///		sdaiRemoveByIterator                                    (https://rdf.bg/ifcdoc/CS64/sdaiRemoveByIterator.html)
		///
		///	The function removes the current member of an aggregate instance, that is not an array, referenced by the specified iterator.
		///	After executing the function, the iterator position set as if the sdaiNext function had been invoked before the member was removed.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemoveByIterator")]
		public static extern void x86_sdaiRemoveByIterator(Int32 iterator);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemoveByIterator")]
		public static extern void x64_sdaiRemoveByIterator(Int64 iterator);

		public static void sdaiRemoveByIterator(Int64 iterator)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiRemoveByIterator((Int32)iterator);
			}
			else
			{
				x64_sdaiRemoveByIterator(iterator);
			}
		}

		/// <summary>
		///		sdaiRemove                                              (https://rdf.bg/ifcdoc/CS64/sdaiRemove.html)
		///
		///	The function removes one occurrence of the specified value from the specified unordered aggregate instance.
		///
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiRemove, and it works similarly for all put-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiRemove but valid for all put-functions)
		///
		///	valueType				C/C++														C#
		///
		///	sdaiINTEGER				int_t val = 123;											int_t val = 123;
		///							sdaiRemove (aggregate, sdaiINTEGER, &val);					ifcengine.sdaiRemove (aggregate, ifcengine.sdaiINTEGER, ref val);
		///
		///	sdaiREAL or sdaiNUMBER	double val = 123.456;										double val = 123.456;
		///							sdaiRemove (aggregate, sdaiREAL, &val);						ifcengine.sdaiRemove (aggregate, ifcengine.sdaiREAL, ref val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val = sdaiTRUE;									bool val = true;
		///							sdaiRemove (aggregate, sdaiBOOLEAN, &val);					ifcengine.sdaiRemove (aggregate, ifcengine.sdaiBOOLEAN, ref val);
		///
		///	sdaiLOGICAL				const TCHAR* val = "U";										string val = "U";
		///							sdaiRemove (aggregate, sdaiLOGICAL, val);					ifcengine.sdaiRemove (aggregate, ifcengine.sdaiLOGICAL, val);
		///
		///	sdaiENUM				const TCHAR* val = "NOTDEFINED";							string val = "NOTDEFINED";
		///							sdaiRemove (aggregate, sdaiENUM, val);						ifcengine.sdaiRemove (aggregate, ifcengine.sdaiENUM, val);
		///
		///	sdaiBINARY				const TCHAR* val = "0123456ABC";							string val = "0123456ABC";
		///							sdaiRemove (aggregate, sdaiBINARY, val);					ifcengine.sdaiRemove (aggregate, ifcengine.sdaiBINARY, val);
		///
		///	sdaiSTRING				const char* val = "My Simple String";						string val = "My Simple String";
		///							sdaiRemove (aggregate, sdaiSTRING, val);					ifcengine.sdaiRemove (aggregate, ifcengine.sdaiSTRING, val);
		///
		///	sdaiUNICODE				const wchar_t* val = L"Any Unicode String";					string val = "Any Unicode String";
		///							sdaiRemove (aggregate, sdaiUNICODE, val);					ifcengine.sdaiRemove (aggregate, ifcengine.sdaiUNICODE, val);
		///
		///	sdaiEXPRESSSTRING		const char* val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";	string val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";
		///							sdaiRemove (aggregate, sdaiEXPRESSSTRING, val);				ifcengine.sdaiRemove (aggregate, ifcengine.sdaiEXPRESSSTRING, val);
		///
		///	sdaiINSTANCE			SdaiInstance val = ...										int_t val = ...
		///							sdaiRemove (aggregate, sdaiINSTANCE, val);					ifcengine.sdaiRemove (aggregate, ifcengine.sdaiINSTANCE, val);
		///
		///	sdaiAGGR				SdaiAggr val = ...											int_t val = ...
		///							sdaiRemove (aggregate, sdaiAGGR, val);						ifcengine.sdaiRemove (aggregate, ifcengine.sdaiAGGR, val);
		///
		///	sdaiADB					SdaiADB val = ...											int_t val = ...
		///							sdaiRemove (aggregate, sdaiADB, val);						ifcengine.sdaiRemove (aggregate, ifcengine.sdaiADB, val);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			 .			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			 .			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			 . 			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiUNICODE			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemove")]
		public static extern void x86_sdaiRemove(Int32 aggregate, Int32 valueType, ref bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemove")]
		public static extern void x64_sdaiRemove(Int64 aggregate, Int64 valueType, ref bool value);

		public static void sdaiRemove(Int64 aggregate, Int64 valueType, ref bool value)
		{
			if (IntPtr.Size == 4)
			{
				bool _value = (bool)value;
				x86_sdaiRemove((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiRemove(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemove")]
		public static extern void x86_sdaiRemove(Int32 aggregate, Int32 valueType, ref Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemove")]
		public static extern void x64_sdaiRemove(Int64 aggregate, Int64 valueType, ref Int64 value);

		public static void sdaiRemove(Int64 aggregate, Int64 valueType, ref Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				Int32 _value = (Int32)value;
				x86_sdaiRemove((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiRemove(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemove")]
		public static extern void x86_sdaiRemove(Int32 aggregate, Int32 valueType, Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemove")]
		public static extern void x64_sdaiRemove(Int64 aggregate, Int64 valueType, Int64 value);

		public static void sdaiRemove(Int64 aggregate, Int64 valueType, Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiRemove((Int32)aggregate, (Int32)valueType, (Int32)value);
			}
			else
			{
				x64_sdaiRemove(aggregate, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemove")]
		public static extern void x86_sdaiRemove(Int32 aggregate, Int32 valueType, ref double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemove")]
		public static extern void x64_sdaiRemove(Int64 aggregate, Int64 valueType, ref double value);

		public static void sdaiRemove(Int64 aggregate, Int64 valueType, ref double value)
		{
			if (IntPtr.Size == 4)
			{
				double _value = (double)value;
				x86_sdaiRemove((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiRemove(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemove")]
		public static extern void x86_sdaiRemove(Int32 aggregate, Int32 valueType, ref IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemove")]
		public static extern void x64_sdaiRemove(Int64 aggregate, Int64 valueType, ref IntPtr value);

		public static void sdaiRemove(Int64 aggregate, Int64 valueType, ref IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				IntPtr _value = (IntPtr)value;
				x86_sdaiRemove((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiRemove(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemove")]
		public static extern void x86_sdaiRemove(Int32 aggregate, Int32 valueType, byte[] value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemove")]
		public static extern void x64_sdaiRemove(Int64 aggregate, Int64 valueType, byte[] value);

		public static void sdaiRemove(Int64 aggregate, Int64 valueType, byte[] value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiRemove((Int32)aggregate, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiRemove(aggregate, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemove")]
		public static extern void x86_sdaiRemove(Int32 aggregate, Int32 valueType, string value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiRemove")]
		public static extern void x64_sdaiRemove(Int64 aggregate, Int64 valueType, string value);

		public static void sdaiRemove(Int64 aggregate, Int64 valueType, string value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiRemove((Int32)aggregate, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiRemove(aggregate, valueType, value);
			}
		}

		/// <summary>
		///		sdaiTestArrayByIndex                                    (https://rdf.bg/ifcdoc/CS64/sdaiTestArrayByIndex.html)
		///
		///	The function tests whether the member of the specified array referenced by the specified index position has a value.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiTestArrayByIndex")]
		public static extern bool x86_sdaiTestArrayByIndex(Int32 aggregate, Int32 index);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiTestArrayByIndex")]
		public static extern bool x64_sdaiTestArrayByIndex(Int64 aggregate, Int64 index);

		public static bool sdaiTestArrayByIndex(Int64 aggregate, Int64 index)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiTestArrayByIndex((Int32)aggregate, (Int32)index);
				return _result;
			}
			else
			{
				return x64_sdaiTestArrayByIndex(aggregate, index);
			}
		}

		/// <summary>
		///		sdaiTestArrayByItr                                      (https://rdf.bg/ifcdoc/CS64/sdaiTestArrayByItr.html)
		///
		///	The function tests whether the member of the specified array referenced by the specified index position has a value.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiTestArrayByItr")]
		public static extern bool x86_sdaiTestArrayByItr(Int32 iterator);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiTestArrayByItr")]
		public static extern bool x64_sdaiTestArrayByItr(Int64 iterator);

		public static bool sdaiTestArrayByItr(Int64 iterator)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiTestArrayByItr((Int32)iterator);
				return _result;
			}
			else
			{
				return x64_sdaiTestArrayByItr(iterator);
			}
		}

		/// <summary>
		///		sdaiCreateInstance                                      (https://rdf.bg/ifcdoc/CS64/sdaiCreateInstance.html)
		///
		///	This call creates an instance of the given entity.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateInstance")]
		public static extern Int32 x86_sdaiCreateInstance(Int32 model, Int32 entity);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateInstance")]
		public static extern Int64 x64_sdaiCreateInstance(Int64 model, Int64 entity);

		public static Int64 sdaiCreateInstance(Int64 model, Int64 entity)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateInstance((Int32)model, (Int32)entity);
				return _result;
			}
			else
			{
				return x64_sdaiCreateInstance(model, entity);
			}
		}

		/// <summary>
		///		sdaiCreateInstanceBN                                    (https://rdf.bg/ifcdoc/CS64/sdaiCreateInstanceBN.html)
		///
		///	This call creates an instance of the given entity.
		///
		///	Technically it will transform into the following call
		///		sdaiCreateInstance(
		///				model,
		///				sdaiGetEntity(
		///						model,
		///						entityName
		///					)
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateInstanceBN")]
		public static extern Int32 x86_sdaiCreateInstanceBN(Int32 model, string entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateInstanceBN")]
		public static extern Int64 x64_sdaiCreateInstanceBN(Int64 model, string entityName);

		public static Int64 sdaiCreateInstanceBN(Int64 model, string entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateInstanceBN((Int32)model, entityName);
				return _result;
			}
			else
			{
				return x64_sdaiCreateInstanceBN(model, entityName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateInstanceBN")]
		public static extern Int32 x86_sdaiCreateInstanceBN(Int32 model, byte[] entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateInstanceBN")]
		public static extern Int64 x64_sdaiCreateInstanceBN(Int64 model, byte[] entityName);

		public static Int64 sdaiCreateInstanceBN(Int64 model, byte[] entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateInstanceBN((Int32)model, entityName);
				return _result;
			}
			else
			{
				return x64_sdaiCreateInstanceBN(model, entityName);
			}
		}

		/// <summary>
		///		sdaiCreateComplexInstance                               (https://rdf.bg/ifcdoc/CS64/sdaiCreateComplexInstance.html)
		///
		///	This call creates a new application instance of the specified type, as determined by a constructed entity type
		///	that is made up of the supplied simple entity types, in the specified SDAI model.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateComplexInstance")]
		public static extern Int32 x86_sdaiCreateComplexInstance(Int32 model, Int32 entityList);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateComplexInstance")]
		public static extern Int64 x64_sdaiCreateComplexInstance(Int64 model, Int64 entityList);

		public static Int64 sdaiCreateComplexInstance(Int64 model, Int64 entityList)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateComplexInstance((Int32)model, (Int32)entityList);
				return _result;
			}
			else
			{
				return x64_sdaiCreateComplexInstance(model, entityList);
			}
		}

		/// <summary>
		///		sdaiCreateComplexInstanceBN                             (https://rdf.bg/ifcdoc/CS64/sdaiCreateComplexInstanceBN.html)
		///
		///	This call creates a new application instance of the specified type, as determined by a constructed entity type
		///	that is made up of the supplied simple entity types, in the specified SDAI model.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateComplexInstanceBN")]
		public static extern Int32 x86_sdaiCreateComplexInstanceBN(Int32 model, Int32 nameNumber, out IntPtr nameVector);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateComplexInstanceBN")]
		public static extern Int64 x64_sdaiCreateComplexInstanceBN(Int64 model, Int64 nameNumber, out IntPtr nameVector);

		public static Int64 sdaiCreateComplexInstanceBN(Int64 model, Int64 nameNumber, out IntPtr nameVector)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateComplexInstanceBN((Int32)model, (Int32)nameNumber, out IntPtr _nameVector);
				nameVector = _nameVector;
				return _result;
			}
			else
			{
				return x64_sdaiCreateComplexInstanceBN(model, nameNumber, out nameVector);
			}
		}

		/// <summary>
		///		sdaiDeleteInstance                                      (https://rdf.bg/ifcdoc/CS64/sdaiDeleteInstance.html)
		///
		///	This call will delete an existing instance.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiDeleteInstance")]
		public static extern void x86_sdaiDeleteInstance(Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiDeleteInstance")]
		public static extern void x64_sdaiDeleteInstance(Int64 instance);

		public static void sdaiDeleteInstance(Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiDeleteInstance((Int32)instance);
			}
			else
			{
				x64_sdaiDeleteInstance(instance);
			}
		}

		/// <summary>
		///		sdaiPutADBTypePath                                      (https://rdf.bg/ifcdoc/CS64/sdaiPutADBTypePath.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBTypePath")]
		public static extern void x86_sdaiPutADBTypePath(Int32 ADB, Int32 pathCount, string path);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBTypePath")]
		public static extern void x64_sdaiPutADBTypePath(Int64 ADB, Int64 pathCount, string path);

		public static void sdaiPutADBTypePath(Int64 ADB, Int64 pathCount, string path)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutADBTypePath((Int32)ADB, (Int32)pathCount, path);
			}
			else
			{
				x64_sdaiPutADBTypePath(ADB, pathCount, path);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBTypePath")]
		public static extern void x86_sdaiPutADBTypePath(Int32 ADB, Int32 pathCount, byte[] path);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutADBTypePath")]
		public static extern void x64_sdaiPutADBTypePath(Int64 ADB, Int64 pathCount, byte[] path);

		public static void sdaiPutADBTypePath(Int64 ADB, Int64 pathCount, byte[] path)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutADBTypePath((Int32)ADB, (Int32)pathCount, path);
			}
			else
			{
				x64_sdaiPutADBTypePath(ADB, pathCount, path);
			}
		}

		/// <summary>
		///		sdaiPutAttr                                             (https://rdf.bg/ifcdoc/CS64/sdaiPutAttr.html)
		///
		///	valueType argument to specify what type of data caller wants to put
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiPutAttr, and it works similarly for all put-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiPutAttr but valid for all put-functions)
		///
		///	valueType				C/C++														C#
		///
		///	sdaiINTEGER				int_t val = 123;											int_t val = 123;
		///							sdaiPutAttr (instance, attribute, sdaiINTEGER, &val);		ifcengine.sdaiPutAttr (instance, attribute, ifcengine.sdaiINTEGER, ref val);
		///
		///	sdaiREAL or sdaiNUMBER	double val = 123.456;										double val = 123.456;
		///							sdaiPutAttr (instance, attribute, sdaiREAL, &val);			ifcengine.sdaiPutAttr (instance, attribute, ifcengine.sdaiREAL, ref val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val = sdaiTRUE;									bool val = true;
		///							sdaiPutAttr (instance, attribute, sdaiBOOLEAN, &val);		ifcengine.sdaiPutAttr (instance, attribute, ifcengine.sdaiBOOLEAN, ref val);
		///
		///	sdaiLOGICAL				const TCHAR* val = "U";										string val = "U";
		///							sdaiPutAttr (instance, attribute, sdaiLOGICAL, val);		ifcengine.sdaiPutAttr (instance, attribute, ifcengine.sdaiLOGICAL, val);
		///
		///	sdaiENUM				const TCHAR* val = "NOTDEFINED";							string val = "NOTDEFINED";
		///							sdaiPutAttr (instance, attribute, sdaiENUM, val);			ifcengine.sdaiPutAttr (instance, attribute, ifcengine.sdaiENUM, val);
		///
		///	sdaiBINARY				const TCHAR* val = "0123456ABC";							string val = "0123456ABC";
		///							sdaiPutAttr (instance, attribute, sdaiBINARY, val);			ifcengine.sdaiPutAttr (instance, attribute, ifcengine.sdaiBINARY, val);
		///
		///	sdaiSTRING				const char* val = "My Simple String";						string val = "My Simple String";
		///							sdaiPutAttr (instance, attribute, sdaiSTRING, val);			ifcengine.sdaiPutAttr (instance, attribute, ifcengine.sdaiSTRING, val);
		///
		///	sdaiUNICODE				const wchar_t* val = L"Any Unicode String";					string val = "Any Unicode String";
		///							sdaiPutAttr (instance, attribute, sdaiUNICODE, val);		ifcengine.sdaiPutAttr (instance, attribute, ifcengine.sdaiUNICODE, val);
		///
		///	sdaiEXPRESSSTRING		const char* val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";	string val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";
		///							sdaiPutAttr (instance, attribute, sdaiEXPRESSSTRING, val);	ifcengine.sdaiPutAttr (instance, attribute, ifcengine.sdaiEXPRESSSTRING, val);
		///
		///	sdaiINSTANCE			SdaiInstance val = sdaiCreateInstanceBN (model, "IFCSITE");	int_t val = ifcengine.sdaiCreateInstanceBN (model, "IFCSITE");
		///							sdaiPutAttr (instance, attribute, sdaiINSTANCE, val);		ifcengine.sdaiPutAttr (instance, attribute, ifcengine.sdaiINSTANCE, val);
		///
		///	sdaiAGGR				SdaiAggr val = sdaiCreateAggr (inst, 0);					int_t val = sdaiCreateAggr (inst, 0);
		///							sdaiPutAttr (val, sdaiINSTANCE, inst);						ifcengine.sdaiPutAttr (val, ifcengine.sdaiINSTANCE, inst);
		///							sdaiPutAttr (instance, attribute, sdaiAGGR, val);			ifcengine.sdaiPutAttr (instance, attribute, ifcengine.sdaiAGGR, val);
		///
		///	sdaiADB					int_t integerValue = 123;									int_t integerValue = 123;	
		///							SdaiADB val = sdaiCreateADB (sdaiINTEGER, &integerValue);	int_t val = ifcengine.sdaiCreateADB (ifcengine.sdaiINTEGER, ref integerValue);
		///							sdaiPutADBTypePath (val, 1, "IFCINTEGER");					ifcengine.sdaiPutADBTypePath (val, 1, "IFCINTEGER");
		///							sdaiPutAttr (instance, attribute, sdaiADB, val);			ifcengine.sdaiPutAttr (instance, attribute, ifcengine.sdaiADB, val);	
		///							sdaiDeleteADB (val);										ifcengine.sdaiDeleteADB (val);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///	(Non-standard extension) sdiADB in C++ has an option to work without sdaiCreateEmptyADB and sdaiDeleteADB as shown in the table.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			 .			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			 .			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			 . 			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiUNICODE			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttr")]
		public static extern void x86_sdaiPutAttr(Int32 instance, Int32 attribute, Int32 valueType, ref bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttr")]
		public static extern void x64_sdaiPutAttr(Int64 instance, Int64 attribute, Int64 valueType, ref bool value);

		public static void sdaiPutAttr(Int64 instance, Int64 attribute, Int64 valueType, ref bool value)
		{
			if (IntPtr.Size == 4)
			{
				bool _value = (bool)value;
				x86_sdaiPutAttr((Int32)instance, (Int32)attribute, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAttr(instance, attribute, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttr")]
		public static extern void x86_sdaiPutAttr(Int32 instance, Int32 attribute, Int32 valueType, ref Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttr")]
		public static extern void x64_sdaiPutAttr(Int64 instance, Int64 attribute, Int64 valueType, ref Int64 value);

		public static void sdaiPutAttr(Int64 instance, Int64 attribute, Int64 valueType, ref Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				Int32 _value = (Int32)value;
				x86_sdaiPutAttr((Int32)instance, (Int32)attribute, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAttr(instance, attribute, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttr")]
		public static extern void x86_sdaiPutAttr(Int32 instance, Int32 attribute, Int32 valueType, Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttr")]
		public static extern void x64_sdaiPutAttr(Int64 instance, Int64 attribute, Int64 valueType, Int64 value);

		public static void sdaiPutAttr(Int64 instance, Int64 attribute, Int64 valueType, Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutAttr((Int32)instance, (Int32)attribute, (Int32)valueType, (Int32)value);
			}
			else
			{
				x64_sdaiPutAttr(instance, attribute, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttr")]
		public static extern void x86_sdaiPutAttr(Int32 instance, Int32 attribute, Int32 valueType, ref double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttr")]
		public static extern void x64_sdaiPutAttr(Int64 instance, Int64 attribute, Int64 valueType, ref double value);

		public static void sdaiPutAttr(Int64 instance, Int64 attribute, Int64 valueType, ref double value)
		{
			if (IntPtr.Size == 4)
			{
				double _value = (double)value;
				x86_sdaiPutAttr((Int32)instance, (Int32)attribute, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAttr(instance, attribute, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttr")]
		public static extern void x86_sdaiPutAttr(Int32 instance, Int32 attribute, Int32 valueType, ref IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttr")]
		public static extern void x64_sdaiPutAttr(Int64 instance, Int64 attribute, Int64 valueType, ref IntPtr value);

		public static void sdaiPutAttr(Int64 instance, Int64 attribute, Int64 valueType, ref IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				IntPtr _value = (IntPtr)value;
				x86_sdaiPutAttr((Int32)instance, (Int32)attribute, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAttr(instance, attribute, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttr")]
		public static extern void x86_sdaiPutAttr(Int32 instance, Int32 attribute, Int32 valueType, byte[] value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttr")]
		public static extern void x64_sdaiPutAttr(Int64 instance, Int64 attribute, Int64 valueType, byte[] value);

		public static void sdaiPutAttr(Int64 instance, Int64 attribute, Int64 valueType, byte[] value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutAttr((Int32)instance, (Int32)attribute, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiPutAttr(instance, attribute, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttr")]
		public static extern void x86_sdaiPutAttr(Int32 instance, Int32 attribute, Int32 valueType, string value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttr")]
		public static extern void x64_sdaiPutAttr(Int64 instance, Int64 attribute, Int64 valueType, string value);

		public static void sdaiPutAttr(Int64 instance, Int64 attribute, Int64 valueType, string value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutAttr((Int32)instance, (Int32)attribute, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiPutAttr(instance, attribute, valueType, value);
			}
		}

		/// <summary>
		///		sdaiPutAttrBN                                           (https://rdf.bg/ifcdoc/CS64/sdaiPutAttrBN.html)
		///
		///	valueType argument to specify what type of data caller wants to put
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiPutAttrBN, and it works similarly for all put-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiPutAttrBN but valid for all put-functions)
		///
		///	valueType				C/C++															C#
		///
		///	sdaiINTEGER				int_t val = 123;												int_t val = 123;
		///							sdaiPutAttrBN (instance, "attrName", sdaiINTEGER, &val);		ifcengine.sdaiPutAttrBN (instance, "attrName", ifcengine.sdaiINTEGER, ref val);
		///
		///	sdaiREAL or sdaiNUMBER	double val = 123.456;											double val = 123.456;
		///							sdaiPutAttrBN (instance, "attrName", sdaiREAL, &val);			ifcengine.sdaiPutAttrBN (instance, "attrName", ifcengine.sdaiREAL, ref val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val = sdaiTRUE;										bool val = true;
		///							sdaiPutAttrBN (instance, "attrName", sdaiBOOLEAN, &val);		ifcengine.sdaiPutAttrBN (instance, "attrName", ifcengine.sdaiBOOLEAN, ref val);
		///
		///	sdaiLOGICAL				const TCHAR* val = "U";											string val = "U";
		///							sdaiPutAttrBN (instance, "attrName", sdaiLOGICAL, val);			ifcengine.sdaiPutAttrBN (instance, "attrName", ifcengine.sdaiLOGICAL, val);
		///
		///	sdaiENUM				const TCHAR* val = "NOTDEFINED";								string val = "NOTDEFINED";
		///							sdaiPutAttrBN (instance, "attrName", sdaiENUM, val);			ifcengine.sdaiPutAttrBN (instance, "attrName", ifcengine.sdaiENUM, val);
		///
		///	sdaiBINARY				const TCHAR* val = "0123456ABC";								string val = "0123456ABC";
		///							sdaiPutAttrBN (instance, "attrName", sdaiBINARY, val);			ifcengine.sdaiPutAttrBN (instance, "attrName", ifcengine.sdaiBINARY, val);
		///
		///	sdaiSTRING				const char* val = "My Simple String";							string val = "My Simple String";
		///							sdaiPutAttrBN (instance, "attrName", sdaiSTRING, val);			ifcengine.sdaiPutAttrBN (instance, "attrName", ifcengine.sdaiSTRING, val);
		///
		///	sdaiUNICODE				const wchar_t* val = L"Any Unicode String";						string val = "Any Unicode String";
		///							sdaiPutAttrBN (instance, "attrName", sdaiUNICODE, val);			ifcengine.sdaiPutAttrBN (instance, "attrName", ifcengine.sdaiUNICODE, val);
		///
		///	sdaiEXPRESSSTRING		const char* val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";		string val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";
		///							sdaiPutAttrBN (instance, "attrName", sdaiEXPRESSSTRING, val);	ifcengine.sdaiPutAttrBN (instance, "attrName", ifcengine.sdaiEXPRESSSTRING, val);
		///
		///	sdaiINSTANCE			SdaiInstance val = sdaiCreateInstanceBN (model, "IFCSITE");		int_t val = ifcengine.sdaiCreateInstanceBN (model, "IFCSITE");
		///							sdaiPutAttrBN (instance, "attrName", sdaiINSTANCE, val);		ifcengine.sdaiPutAttrBN (instance, "attrName", ifcengine.sdaiINSTANCE, val);
		///
		///	sdaiAGGR				SdaiAggr val = sdaiCreateAggr (inst, 0);						int_t val = sdaiCreateAggr (inst, 0);
		///							sdaiPutAttr (val, sdaiINSTANCE, inst);							ifcengine.sdaiPutAttr (val, ifcengine.sdaiINSTANCE, inst);
		///							sdaiPutAttrBN (instance, "attrName", sdaiAGGR, val);			ifcengine.sdaiPutAttrBN (instance, "attrName", ifcengine.sdaiAGGR, val);
		///
		///	sdaiADB					int_t integerValue = 123;										int_t integerValue = 123;	
		///							SdaiADB val = sdaiCreateADB (sdaiINTEGER, &integerValue);		int_t val = ifcengine.sdaiCreateADB (ifcengine.sdaiINTEGER, ref integerValue);
		///							sdaiPutADBTypePath (val, 1, "IFCINTEGER");						ifcengine.sdaiPutADBTypePath (val, 1, "IFCINTEGER");
		///							sdaiPutAttrBN (instance, "attrName", sdaiADB, val);				ifcengine.sdaiPutAttrBN (instance, "attrName", ifcengine.sdaiADB, val);	
		///							sdaiDeleteADB (val);											ifcengine.sdaiDeleteADB (val);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///	(Non-standard extension) sdiADB in C++ has an option to work without sdaiCreateEmptyADB and sdaiDeleteADB as shown in the table.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			 .			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			 .			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			 . 			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiUNICODE			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		///
		///	Technically sdaiPutAttrBN will transform into the following call
		///		sdaiPutAttr(
		///				instance,
		///				sdaiGetAttrDefinition(
		///						sdaiGetInstanceType(
		///								instance
		///							),
		///						attributeName
		///					),
		///				valueType,
		///				value
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x86_sdaiPutAttrBN(Int32 instance, string attributeName, Int32 valueType, ref bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x64_sdaiPutAttrBN(Int64 instance, string attributeName, Int64 valueType, ref bool value);

		public static void sdaiPutAttrBN(Int64 instance, string attributeName, Int64 valueType, ref bool value)
		{
			if (IntPtr.Size == 4)
			{
				bool _value = (bool)value;
				x86_sdaiPutAttrBN((Int32)instance, attributeName, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAttrBN(instance, attributeName, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x86_sdaiPutAttrBN(Int32 instance, string attributeName, Int32 valueType, ref Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x64_sdaiPutAttrBN(Int64 instance, string attributeName, Int64 valueType, ref Int64 value);

		public static void sdaiPutAttrBN(Int64 instance, string attributeName, Int64 valueType, ref Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				Int32 _value = (Int32)value;
				x86_sdaiPutAttrBN((Int32)instance, attributeName, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAttrBN(instance, attributeName, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x86_sdaiPutAttrBN(Int32 instance, string attributeName, Int32 valueType, Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x64_sdaiPutAttrBN(Int64 instance, string attributeName, Int64 valueType, Int64 value);

		public static void sdaiPutAttrBN(Int64 instance, string attributeName, Int64 valueType, Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutAttrBN((Int32)instance, attributeName, (Int32)valueType, (Int32)value);
			}
			else
			{
				x64_sdaiPutAttrBN(instance, attributeName, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x86_sdaiPutAttrBN(Int32 instance, string attributeName, Int32 valueType, ref double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x64_sdaiPutAttrBN(Int64 instance, string attributeName, Int64 valueType, ref double value);

		public static void sdaiPutAttrBN(Int64 instance, string attributeName, Int64 valueType, ref double value)
		{
			if (IntPtr.Size == 4)
			{
				double _value = (double)value;
				x86_sdaiPutAttrBN((Int32)instance, attributeName, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAttrBN(instance, attributeName, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x86_sdaiPutAttrBN(Int32 instance, string attributeName, Int32 valueType, ref IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x64_sdaiPutAttrBN(Int64 instance, string attributeName, Int64 valueType, ref IntPtr value);

		public static void sdaiPutAttrBN(Int64 instance, string attributeName, Int64 valueType, ref IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				IntPtr _value = (IntPtr)value;
				x86_sdaiPutAttrBN((Int32)instance, attributeName, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAttrBN(instance, attributeName, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x86_sdaiPutAttrBN(Int32 instance, string attributeName, Int32 valueType, byte[] value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x64_sdaiPutAttrBN(Int64 instance, string attributeName, Int64 valueType, byte[] value);

		public static void sdaiPutAttrBN(Int64 instance, string attributeName, Int64 valueType, byte[] value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutAttrBN((Int32)instance, attributeName, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiPutAttrBN(instance, attributeName, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x86_sdaiPutAttrBN(Int32 instance, string attributeName, Int32 valueType, string value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x64_sdaiPutAttrBN(Int64 instance, string attributeName, Int64 valueType, string value);

		public static void sdaiPutAttrBN(Int64 instance, string attributeName, Int64 valueType, string value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutAttrBN((Int32)instance, attributeName, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiPutAttrBN(instance, attributeName, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x86_sdaiPutAttrBN(Int32 instance, byte[] attributeName, Int32 valueType, ref bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x64_sdaiPutAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, ref bool value);

		public static void sdaiPutAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, ref bool value)
		{
			if (IntPtr.Size == 4)
			{
				bool _value = (bool)value;
				x86_sdaiPutAttrBN((Int32)instance, attributeName, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAttrBN(instance, attributeName, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x86_sdaiPutAttrBN(Int32 instance, byte[] attributeName, Int32 valueType, ref Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x64_sdaiPutAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, ref Int64 value);

		public static void sdaiPutAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, ref Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				Int32 _value = (Int32)value;
				x86_sdaiPutAttrBN((Int32)instance, attributeName, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAttrBN(instance, attributeName, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x86_sdaiPutAttrBN(Int32 instance, byte[] attributeName, Int32 valueType, Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x64_sdaiPutAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, Int64 value);

		public static void sdaiPutAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutAttrBN((Int32)instance, attributeName, (Int32)valueType, (Int32)value);
			}
			else
			{
				x64_sdaiPutAttrBN(instance, attributeName, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x86_sdaiPutAttrBN(Int32 instance, byte[] attributeName, Int32 valueType, ref double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x64_sdaiPutAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, ref double value);

		public static void sdaiPutAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, ref double value)
		{
			if (IntPtr.Size == 4)
			{
				double _value = (double)value;
				x86_sdaiPutAttrBN((Int32)instance, attributeName, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAttrBN(instance, attributeName, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x86_sdaiPutAttrBN(Int32 instance, byte[] attributeName, Int32 valueType, ref IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x64_sdaiPutAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, ref IntPtr value);

		public static void sdaiPutAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, ref IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				IntPtr _value = (IntPtr)value;
				x86_sdaiPutAttrBN((Int32)instance, attributeName, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAttrBN(instance, attributeName, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x86_sdaiPutAttrBN(Int32 instance, byte[] attributeName, Int32 valueType, byte[] value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x64_sdaiPutAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, byte[] value);

		public static void sdaiPutAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, byte[] value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutAttrBN((Int32)instance, attributeName, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiPutAttrBN(instance, attributeName, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x86_sdaiPutAttrBN(Int32 instance, byte[] attributeName, Int32 valueType, string value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAttrBN")]
		public static extern void x64_sdaiPutAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, string value);

		public static void sdaiPutAttrBN(Int64 instance, byte[] attributeName, Int64 valueType, string value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutAttrBN((Int32)instance, attributeName, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiPutAttrBN(instance, attributeName, valueType, value);
			}
		}

		/// <summary>
		///		sdaiUnsetAttr                                           (https://rdf.bg/ifcdoc/CS64/sdaiUnsetAttr.html)
		///
		///	This call removes all data from a specific attribute for the given instance.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiUnsetAttr")]
		public static extern void x86_sdaiUnsetAttr(Int32 instance, Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiUnsetAttr")]
		public static extern void x64_sdaiUnsetAttr(Int64 instance, Int64 attribute);

		public static void sdaiUnsetAttr(Int64 instance, Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiUnsetAttr((Int32)instance, (Int32)attribute);
			}
			else
			{
				x64_sdaiUnsetAttr(instance, attribute);
			}
		}

		/// <summary>
		///		sdaiUnsetAttrBN                                         (https://rdf.bg/ifcdoc/CS64/sdaiUnsetAttrBN.html)
		///
		///	This call removes all data from a specific attribute for the given instance.
		///
		///	Technically it will transform into the following call
		///		sdaiUnsetAttr(
		///				instance,
		///				sdaiGetAttrDefinition(
		///						sdaiGetInstanceType(
		///								instance
		///							),
		///						attributeName
		///					)
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiUnsetAttrBN")]
		public static extern void x86_sdaiUnsetAttrBN(Int32 instance, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiUnsetAttrBN")]
		public static extern void x64_sdaiUnsetAttrBN(Int64 instance, string attributeName);

		public static void sdaiUnsetAttrBN(Int64 instance, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiUnsetAttrBN((Int32)instance, attributeName);
			}
			else
			{
				x64_sdaiUnsetAttrBN(instance, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiUnsetAttrBN")]
		public static extern void x86_sdaiUnsetAttrBN(Int32 instance, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiUnsetAttrBN")]
		public static extern void x64_sdaiUnsetAttrBN(Int64 instance, byte[] attributeName);

		public static void sdaiUnsetAttrBN(Int64 instance, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiUnsetAttrBN((Int32)instance, attributeName);
			}
			else
			{
				x64_sdaiUnsetAttrBN(instance, attributeName);
			}
		}

		/// <summary>
		///		engiSetComment                                          (https://rdf.bg/ifcdoc/CS64/engiSetComment.html)
		///
		///	This call can be used to add a comment to an instance when exporting the content. The comment is available in the exported/saved IFC file.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiSetComment")]
		public static extern void x86_engiSetComment(Int32 instance, string comment);

		[DllImport(IFCEngineDLL, EntryPoint = "engiSetComment")]
		public static extern void x64_engiSetComment(Int64 instance, string comment);

		public static void engiSetComment(Int64 instance, string comment)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiSetComment((Int32)instance, comment);
			}
			else
			{
				x64_engiSetComment(instance, comment);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiSetComment")]
		public static extern void x86_engiSetComment(Int32 instance, byte[] comment);

		[DllImport(IFCEngineDLL, EntryPoint = "engiSetComment")]
		public static extern void x64_engiSetComment(Int64 instance, byte[] comment);

		public static void engiSetComment(Int64 instance, byte[] comment)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiSetComment((Int32)instance, comment);
			}
			else
			{
				x64_engiSetComment(instance, comment);
			}
		}

		/// <summary>
		///		engiGetInstanceLocalId                                  (https://rdf.bg/ifcdoc/CS64/engiGetInstanceLocalId.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetInstanceLocalId")]
		public static extern Int64 x86_engiGetInstanceLocalId(Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetInstanceLocalId")]
		public static extern Int64 x64_engiGetInstanceLocalId(Int64 instance);

		public static Int64 engiGetInstanceLocalId(Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetInstanceLocalId((Int32)instance);
				return _result;
			}
			else
			{
				return x64_engiGetInstanceLocalId(instance);
			}
		}

		/// <summary>
		///		sdaiTestAttr                                            (https://rdf.bg/ifcdoc/CS64/sdaiTestAttr.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiTestAttr")]
		public static extern Int32 x86_sdaiTestAttr(Int32 instance, Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiTestAttr")]
		public static extern Int64 x64_sdaiTestAttr(Int64 instance, Int64 attribute);

		public static Int64 sdaiTestAttr(Int64 instance, Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiTestAttr((Int32)instance, (Int32)attribute);
				return _result;
			}
			else
			{
				return x64_sdaiTestAttr(instance, attribute);
			}
		}

		/// <summary>
		///		sdaiTestAttrBN                                          (https://rdf.bg/ifcdoc/CS64/sdaiTestAttrBN.html)
		///
		///	Technically it will transform into the following call
		///		sdaiGetAttrDefinition(
		///				sdaiGetInstanceType(
		///						instance
		///					),
		///				attributeName
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiTestAttrBN")]
		public static extern Int32 x86_sdaiTestAttrBN(Int32 instance, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiTestAttrBN")]
		public static extern Int64 x64_sdaiTestAttrBN(Int64 instance, string attributeName);

		public static Int64 sdaiTestAttrBN(Int64 instance, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiTestAttrBN((Int32)instance, attributeName);
				return _result;
			}
			else
			{
				return x64_sdaiTestAttrBN(instance, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiTestAttrBN")]
		public static extern Int32 x86_sdaiTestAttrBN(Int32 instance, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiTestAttrBN")]
		public static extern Int64 x64_sdaiTestAttrBN(Int64 instance, byte[] attributeName);

		public static Int64 sdaiTestAttrBN(Int64 instance, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiTestAttrBN((Int32)instance, attributeName);
				return _result;
			}
			else
			{
				return x64_sdaiTestAttrBN(instance, attributeName);
			}
		}

		/// <summary>
		///		sdaiCreateInstanceEI                                    (https://rdf.bg/ifcdoc/CS64/sdaiCreateInstanceEI.html)
		///
		///	This call creates an instance at a specific given express ID, the instance is only created if the express ID was not used yet.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateInstanceEI")]
		public static extern Int32 x86_sdaiCreateInstanceEI(Int32 model, Int32 entity, Int64 expressID);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateInstanceEI")]
		public static extern Int64 x64_sdaiCreateInstanceEI(Int64 model, Int64 entity, Int64 expressID);

		public static Int64 sdaiCreateInstanceEI(Int64 model, Int64 entity, Int64 expressID)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateInstanceEI((Int32)model, (Int32)entity, expressID);
				return _result;
			}
			else
			{
				return x64_sdaiCreateInstanceEI(model, entity, expressID);
			}
		}

		/// <summary>
		///		sdaiCreateInstanceBNEI                                  (https://rdf.bg/ifcdoc/CS64/sdaiCreateInstanceBNEI.html)
		///
		///	This call creates an instance at a specific given express ID, the instance is only created if the express ID was not used yet.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateInstanceBNEI")]
		public static extern Int32 x86_sdaiCreateInstanceBNEI(Int32 model, string entityName, Int64 expressID);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateInstanceBNEI")]
		public static extern Int64 x64_sdaiCreateInstanceBNEI(Int64 model, string entityName, Int64 expressID);

		public static Int64 sdaiCreateInstanceBNEI(Int64 model, string entityName, Int64 expressID)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateInstanceBNEI((Int32)model, entityName, expressID);
				return _result;
			}
			else
			{
				return x64_sdaiCreateInstanceBNEI(model, entityName, expressID);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateInstanceBNEI")]
		public static extern Int32 x86_sdaiCreateInstanceBNEI(Int32 model, byte[] entityName, Int64 expressID);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateInstanceBNEI")]
		public static extern Int64 x64_sdaiCreateInstanceBNEI(Int64 model, byte[] entityName, Int64 expressID);

		public static Int64 sdaiCreateInstanceBNEI(Int64 model, byte[] entityName, Int64 expressID)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateInstanceBNEI((Int32)model, entityName, expressID);
				return _result;
			}
			else
			{
				return x64_sdaiCreateInstanceBNEI(model, entityName, expressID);
			}
		}

		/// <summary>
		///		sdaiCreateIterator                                      (https://rdf.bg/ifcdoc/CS64/sdaiCreateIterator.html)
		///
		///	This function creates an iterator associated with the specified aggregate instance.
		///	The iterator is positioned as if the sdaiBeginning function had been executed such that so that no
		///	member of the aggregate is referenced as the current member.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateIterator")]
		public static extern Int32 x86_sdaiCreateIterator(Int32 aggregate);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiCreateIterator")]
		public static extern Int64 x64_sdaiCreateIterator(Int64 aggregate);

		public static Int64 sdaiCreateIterator(Int64 aggregate)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiCreateIterator((Int32)aggregate);
				return _result;
			}
			else
			{
				return x64_sdaiCreateIterator(aggregate);
			}
		}

		/// <summary>
		///		sdaiDeleteIterator                                      (https://rdf.bg/ifcdoc/CS64/sdaiDeleteIterator.html)
		///
		///	This function deletes the specified iterator.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiDeleteIterator")]
		public static extern void x86_sdaiDeleteIterator(Int32 iterator);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiDeleteIterator")]
		public static extern void x64_sdaiDeleteIterator(Int64 iterator);

		public static void sdaiDeleteIterator(Int64 iterator)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiDeleteIterator((Int32)iterator);
			}
			else
			{
				x64_sdaiDeleteIterator(iterator);
			}
		}

		/// <summary>
		///		sdaiBeginning                                           (https://rdf.bg/ifcdoc/CS64/sdaiBeginning.html)
		///
		///	The function positions the iterator at the beginning of its associated aggregate instance such that there is no current member.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiBeginning")]
		public static extern void x86_sdaiBeginning(Int32 iterator);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiBeginning")]
		public static extern void x64_sdaiBeginning(Int64 iterator);

		public static void sdaiBeginning(Int64 iterator)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiBeginning((Int32)iterator);
			}
			else
			{
				x64_sdaiBeginning(iterator);
			}
		}

		/// <summary>
		///		sdaiNext                                                (https://rdf.bg/ifcdoc/CS64/sdaiNext.html)
		///
		///	This function positions the iterator to the succeeding member of the associated aggregate instance.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiNext")]
		public static extern bool x86_sdaiNext(Int32 iterator);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiNext")]
		public static extern bool x64_sdaiNext(Int64 iterator);

		public static bool sdaiNext(Int64 iterator)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiNext((Int32)iterator);
				return _result;
			}
			else
			{
				return x64_sdaiNext(iterator);
			}
		}

		/// <summary>
		///		sdaiPrevious                                            (https://rdf.bg/ifcdoc/CS64/sdaiPrevious.html)
		///
		///	This function positions the specified iterator so that the preceding member of its subject
		///	ordered aggregate instance shall become the current member.
		///	If the iterator is at the end of the aggregate, the last member becomes the current member.
		///	If the iterator is at the beginning of the aggregate no repositioning occur.
		///	If the iterator references the first member of the aggregate, the iterator is set at the beginning so there is no current member.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPrevious")]
		public static extern Int32 x86_sdaiPrevious(Int32 iterator);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPrevious")]
		public static extern Int64 x64_sdaiPrevious(Int64 iterator);

		public static Int64 sdaiPrevious(Int64 iterator)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiPrevious((Int32)iterator);
				return _result;
			}
			else
			{
				return x64_sdaiPrevious(iterator);
			}
		}

		/// <summary>
		///		sdaiEnd                                                 (https://rdf.bg/ifcdoc/CS64/sdaiEnd.html)
		///
		///	This function positions the specified iterator at the end of the ordered aggregate instance members such that there is no current member.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiEnd")]
		public static extern void x86_sdaiEnd(Int32 iterator);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiEnd")]
		public static extern void x64_sdaiEnd(Int64 iterator);

		public static void sdaiEnd(Int64 iterator)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiEnd((Int32)iterator);
			}
			else
			{
				x64_sdaiEnd(iterator);
			}
		}

		/// <summary>
		///		sdaiIsMember                                            (https://rdf.bg/ifcdoc/CS64/sdaiIsMember.html)
		///
		///	The function determines whether the specified primitive or instance value is contained
		///	in the aggregate. In the case of aggregate members represented by ADBs, both the data value and data
		///	type are compared.
		///
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiIsMember, and it works similarly for all put-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiIsMember but valid for all put-functions)
		///
		///	valueType				C/C++														C#
		///
		///	sdaiINTEGER				int_t val = 123;											int_t val = 123;
		///							sdaiIsMember (sdaiINTEGER, &val);							ifcengine.sdaiIsMember (ifcengine.sdaiINTEGER, ref val);
		///
		///	sdaiREAL or sdaiNUMBER	double val = 123.456;										double val = 123.456;
		///							sdaiIsMember (sdaiREAL, &val);								ifcengine.sdaiIsMember (ifcengine.sdaiREAL, ref val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val = sdaiTRUE;									bool val = true;
		///							sdaiIsMember (sdaiBOOLEAN, &val);							ifcengine.sdaiIsMember (ifcengine.sdaiBOOLEAN, ref val);
		///
		///	sdaiLOGICAL				const TCHAR* val = "U";										string val = "U";
		///							sdaiIsMember (sdaiLOGICAL, val);							ifcengine.sdaiIsMember (ifcengine.sdaiLOGICAL, val);
		///
		///	sdaiENUM				const TCHAR* val = "NOTDEFINED";							string val = "NOTDEFINED";
		///							sdaiIsMember (sdaiENUM, val);								ifcengine.sdaiIsMember (ifcengine.sdaiENUM, val);
		///
		///	sdaiBINARY				const TCHAR* val = "0123456ABC";							string val = "0123456ABC";
		///							sdaiIsMember (sdaiBINARY, val);								ifcengine.sdaiIsMember (ifcengine.sdaiBINARY, val);
		///
		///	sdaiSTRING				const char* val = "My Simple String";						string val = "My Simple String";
		///							sdaiIsMember (sdaiSTRING, val);								ifcengine.sdaiIsMember (ifcengine.sdaiSTRING, val);
		///
		///	sdaiUNICODE				const wchar_t* val = L"Any Unicode String";					string val = "Any Unicode String";
		///							sdaiIsMember (sdaiUNICODE, val);							ifcengine.sdaiIsMember (ifcengine.sdaiUNICODE, val);
		///
		///	sdaiEXPRESSSTRING		const char* val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";	string val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";
		///							sdaiIsMember (sdaiEXPRESSSTRING, val);						ifcengine.sdaiIsMember (ifcengine.sdaiEXPRESSSTRING, val);
		///
		///	sdaiINSTANCE			SdaiInstance val = ...										int_t val = ...
		///							sdaiIsMember (sdaiINSTANCE, val);							ifcengine.sdaiIsMember (ifcengine.sdaiINSTANCE, val);
		///
		///	sdaiAGGR				SdaiAggr val = ...											int_t val = ...
		///							sdaiIsMember (sdaiAGGR, val);								ifcengine.sdaiIsMember (ifcengine.sdaiAGGR, val);
		///
		///	sdaiADB					SdaiADB val = ...											int_t val = ...
		///							sdaiIsMember (sdaiADB, val);								ifcengine.sdaiIsMember (ifcengine.sdaiADB, val);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			 .			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			 .			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			 . 			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiUNICODE			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsMember")]
		public static extern bool x86_sdaiIsMember(Int32 aggregate, Int32 valueType, ref bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsMember")]
		public static extern bool x64_sdaiIsMember(Int64 aggregate, Int64 valueType, ref bool value);

		public static bool sdaiIsMember(Int64 aggregate, Int64 valueType, ref bool value)
		{
			if (IntPtr.Size == 4)
			{
				bool _value = (bool)value;
				var _result = x86_sdaiIsMember((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiIsMember(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsMember")]
		public static extern bool x86_sdaiIsMember(Int32 aggregate, Int32 valueType, ref Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsMember")]
		public static extern bool x64_sdaiIsMember(Int64 aggregate, Int64 valueType, ref Int64 value);

		public static bool sdaiIsMember(Int64 aggregate, Int64 valueType, ref Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				Int32 _value = (Int32)value;
				var _result = x86_sdaiIsMember((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiIsMember(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsMember")]
		public static extern bool x86_sdaiIsMember(Int32 aggregate, Int32 valueType, Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsMember")]
		public static extern bool x64_sdaiIsMember(Int64 aggregate, Int64 valueType, Int64 value);

		public static bool sdaiIsMember(Int64 aggregate, Int64 valueType, Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiIsMember((Int32)aggregate, (Int32)valueType, (Int32)value);
				return _result;
			}
			else
			{
				return x64_sdaiIsMember(aggregate, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsMember")]
		public static extern bool x86_sdaiIsMember(Int32 aggregate, Int32 valueType, ref double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsMember")]
		public static extern bool x64_sdaiIsMember(Int64 aggregate, Int64 valueType, ref double value);

		public static bool sdaiIsMember(Int64 aggregate, Int64 valueType, ref double value)
		{
			if (IntPtr.Size == 4)
			{
				double _value = (double)value;
				var _result = x86_sdaiIsMember((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiIsMember(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsMember")]
		public static extern bool x86_sdaiIsMember(Int32 aggregate, Int32 valueType, ref IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsMember")]
		public static extern bool x64_sdaiIsMember(Int64 aggregate, Int64 valueType, ref IntPtr value);

		public static bool sdaiIsMember(Int64 aggregate, Int64 valueType, ref IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				IntPtr _value = (IntPtr)value;
				var _result = x86_sdaiIsMember((Int32)aggregate, (Int32)valueType, ref _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiIsMember(aggregate, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsMember")]
		public static extern bool x86_sdaiIsMember(Int32 aggregate, Int32 valueType, byte[] value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsMember")]
		public static extern bool x64_sdaiIsMember(Int64 aggregate, Int64 valueType, byte[] value);

		public static bool sdaiIsMember(Int64 aggregate, Int64 valueType, byte[] value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiIsMember((Int32)aggregate, (Int32)valueType, value);
				return _result;
			}
			else
			{
				return x64_sdaiIsMember(aggregate, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsMember")]
		public static extern bool x86_sdaiIsMember(Int32 aggregate, Int32 valueType, string value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiIsMember")]
		public static extern bool x64_sdaiIsMember(Int64 aggregate, Int64 valueType, string value);

		public static bool sdaiIsMember(Int64 aggregate, Int64 valueType, string value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiIsMember((Int32)aggregate, (Int32)valueType, value);
				return _result;
			}
			else
			{
				return x64_sdaiIsMember(aggregate, valueType, value);
			}
		}

		/// <summary>
		///		sdaiGetAggrElementBoundByItr                            (https://rdf.bg/ifcdoc/CS64/sdaiGetAggrElementBoundByItr.html)
		///
		///	The function returns the current value of the real precision, the string width, or the binary width
		///	for the current member referenced by the specified iterator.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrElementBoundByItr")]
		public static extern Int32 x86_sdaiGetAggrElementBoundByItr(Int32 iterator);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrElementBoundByItr")]
		public static extern Int64 x64_sdaiGetAggrElementBoundByItr(Int64 iterator);

		public static Int64 sdaiGetAggrElementBoundByItr(Int64 iterator)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAggrElementBoundByItr((Int32)iterator);
				return _result;
			}
			else
			{
				return x64_sdaiGetAggrElementBoundByItr(iterator);
			}
		}

		/// <summary>
		///		sdaiGetAggrElementBoundByIndex                          (https://rdf.bg/ifcdoc/CS64/sdaiGetAggrElementBoundByIndex.html)
		///
		///	The function returns the current value of the real precision, the string width, or the binary width 
		///	of the aggregate element at the specified index position in the specified ordered aggregate instance.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrElementBoundByIndex")]
		public static extern Int32 x86_sdaiGetAggrElementBoundByIndex(Int32 aggregate, Int32 index);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrElementBoundByIndex")]
		public static extern Int64 x64_sdaiGetAggrElementBoundByIndex(Int64 aggregate, Int64 index);

		public static Int64 sdaiGetAggrElementBoundByIndex(Int64 aggregate, Int64 index)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAggrElementBoundByIndex((Int32)aggregate, (Int32)index);
				return _result;
			}
			else
			{
				return x64_sdaiGetAggrElementBoundByIndex(aggregate, index);
			}
		}

		/// <summary>
		///		sdaiGetLowerBound                                       (https://rdf.bg/ifcdoc/CS64/sdaiGetLowerBound.html)
		///
		///	The function returns the current value of the lower bound, or index, of the specified aggregate instance.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetLowerBound")]
		public static extern Int32 x86_sdaiGetLowerBound(Int32 aggregate);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetLowerBound")]
		public static extern Int64 x64_sdaiGetLowerBound(Int64 aggregate);

		public static Int64 sdaiGetLowerBound(Int64 aggregate)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetLowerBound((Int32)aggregate);
				return _result;
			}
			else
			{
				return x64_sdaiGetLowerBound(aggregate);
			}
		}

		/// <summary>
		///		sdaiGetUpperBound                                       (https://rdf.bg/ifcdoc/CS64/sdaiGetUpperBound.html)
		///
		///	The function returns the current value of the upper bound, or index, of the specified aggregate instance.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetUpperBound")]
		public static extern Int32 x86_sdaiGetUpperBound(Int32 aggregate);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetUpperBound")]
		public static extern Int64 x64_sdaiGetUpperBound(Int64 aggregate);

		public static Int64 sdaiGetUpperBound(Int64 aggregate)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetUpperBound((Int32)aggregate);
				return _result;
			}
			else
			{
				return x64_sdaiGetUpperBound(aggregate);
			}
		}

		/// <summary>
		///		sdaiGetLowerIndex                                       (https://rdf.bg/ifcdoc/CS64/sdaiGetLowerIndex.html)
		///
		///	The function returns the value of the lower index of the specified array instance when it was created.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetLowerIndex")]
		public static extern Int32 x86_sdaiGetLowerIndex(Int32 aggregate);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetLowerIndex")]
		public static extern Int64 x64_sdaiGetLowerIndex(Int64 aggregate);

		public static Int64 sdaiGetLowerIndex(Int64 aggregate)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetLowerIndex((Int32)aggregate);
				return _result;
			}
			else
			{
				return x64_sdaiGetLowerIndex(aggregate);
			}
		}

		/// <summary>
		///		sdaiGetUpperIndex                                       (https://rdf.bg/ifcdoc/CS64/sdaiGetUpperIndex.html)
		///
		///	The function returns the value of the upper index of the specified array instance when it was created.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetUpperIndex")]
		public static extern Int32 x86_sdaiGetUpperIndex(Int32 aggregate);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetUpperIndex")]
		public static extern Int64 x64_sdaiGetUpperIndex(Int64 aggregate);

		public static Int64 sdaiGetUpperIndex(Int64 aggregate)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetUpperIndex((Int32)aggregate);
				return _result;
			}
			else
			{
				return x64_sdaiGetUpperIndex(aggregate);
			}
		}

		/// <summary>
		///		sdaiUnsetArrayByIndex                                   (https://rdf.bg/ifcdoc/CS64/sdaiUnsetArrayByIndex.html)
		///
		///	The function restores the unset (not assigned a value) status of the member
		///	of the specified array at the specified index position.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiUnsetArrayByIndex")]
		public static extern void x86_sdaiUnsetArrayByIndex(Int32 array, Int32 index);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiUnsetArrayByIndex")]
		public static extern void x64_sdaiUnsetArrayByIndex(Int64 array, Int64 index);

		public static void sdaiUnsetArrayByIndex(Int64 array, Int64 index)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiUnsetArrayByIndex((Int32)array, (Int32)index);
			}
			else
			{
				x64_sdaiUnsetArrayByIndex(array, index);
			}
		}

		/// <summary>
		///		sdaiUnsetArrayByItr                                     (https://rdf.bg/ifcdoc/CS64/sdaiUnsetArrayByItr.html)
		///
		///	The function restores the unset (not assigned a value) status of a member at the
		///	position identified by the iterator in the array associated with the iterator.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiUnsetArrayByItr")]
		public static extern void x86_sdaiUnsetArrayByItr(Int32 iterator);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiUnsetArrayByItr")]
		public static extern void x64_sdaiUnsetArrayByItr(Int64 iterator);

		public static void sdaiUnsetArrayByItr(Int64 iterator)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiUnsetArrayByItr((Int32)iterator);
			}
			else
			{
				x64_sdaiUnsetArrayByItr(iterator);
			}
		}

		/// <summary>
		///		sdaiReindexArray                                        (https://rdf.bg/ifcdoc/CS64/sdaiReindexArray.html)
		///
		///	The function resizes the specified array instance setting the lower, or upper index,
		///	or both, based upon the current population of the application schema.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiReindexArray")]
		public static extern void x86_sdaiReindexArray(Int32 array);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiReindexArray")]
		public static extern void x64_sdaiReindexArray(Int64 array);

		public static void sdaiReindexArray(Int64 array)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiReindexArray((Int32)array);
			}
			else
			{
				x64_sdaiReindexArray(array);
			}
		}

		/// <summary>
		///		sdaiResetArrayIndex                                     (https://rdf.bg/ifcdoc/CS64/sdaiResetArrayIndex.html)
		///
		///	The function shall resizes the specified array instance setting the lower and upper
		///	index with the specified values.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiResetArrayIndex")]
		public static extern void x86_sdaiResetArrayIndex(Int32 array, Int32 lower, Int32 upper);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiResetArrayIndex")]
		public static extern void x64_sdaiResetArrayIndex(Int64 array, Int64 lower, Int64 upper);

		public static void sdaiResetArrayIndex(Int64 array, Int64 lower, Int64 upper)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiResetArrayIndex((Int32)array, (Int32)lower, (Int32)upper);
			}
			else
			{
				x64_sdaiResetArrayIndex(array, lower, upper);
			}
		}

		/// <summary>
		///		engiEnableDerivedAttributes                             (https://rdf.bg/ifcdoc/CS64/engiEnableDerivedAttributes.html)
		///
		///	The function enables calculation of derived attributes for sdaiGetAttr(BN) and other get value functions and dynamic aggregation indexes.
		///	Returns success flag.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiEnableDerivedAttributes")]
		public static extern bool x86_engiEnableDerivedAttributes(Int32 model, bool enable);

		[DllImport(IFCEngineDLL, EntryPoint = "engiEnableDerivedAttributes")]
		public static extern bool x64_engiEnableDerivedAttributes(Int64 model, bool enable);

		public static bool engiEnableDerivedAttributes(Int64 model, bool enable)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiEnableDerivedAttributes((Int32)model, enable);
				return _result;
			}
			else
			{
				return x64_engiEnableDerivedAttributes(model, enable);
			}
		}

		/// <summary>
		///		engiEvaluateAllDerivedAttributes                        (https://rdf.bg/ifcdoc/CS64/engiEvaluateAllDerivedAttributes.html)
		///
		///	The function evaluates and replaces all * with values, optionally can handle $ values as derived attributes.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiEvaluateAllDerivedAttributes")]
		public static extern void x86_engiEvaluateAllDerivedAttributes(Int32 model, bool includeNullValues);

		[DllImport(IFCEngineDLL, EntryPoint = "engiEvaluateAllDerivedAttributes")]
		public static extern void x64_engiEvaluateAllDerivedAttributes(Int64 model, bool includeNullValues);

		public static void engiEvaluateAllDerivedAttributes(Int64 model, bool includeNullValues)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiEvaluateAllDerivedAttributes((Int32)model, includeNullValues);
			}
			else
			{
				x64_engiEvaluateAllDerivedAttributes(model, includeNullValues);
			}
		}

		/// <summary>
		///		setSegmentation                                         (https://rdf.bg/ifcdoc/CS64/setSegmentation.html)
		///
		///	This call sets the segmentation for any curved part of an object in case it is defined by a circle, ellipse, nurbs etc.
		///
		///	If segmentationParts is set to 0 it will fallback on the default setting (i.e. 36),
		///	it makes sense to change the segmentation depending on the entity type that is visualized.
		///
		///	in case segmentationLength is non-zero, this is the maximum length (in file length unit definition) of a segment
		///	For example a slightly curved wall with large size will get much more precise segmentation as the segmentLength
		///	will force the segmentation for the wall to increase.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "setSegmentation")]
		public static extern void x86_setSegmentation(Int32 model, Int32 segmentationParts, double segmentationLength);

		[DllImport(IFCEngineDLL, EntryPoint = "setSegmentation")]
		public static extern void x64_setSegmentation(Int64 model, Int64 segmentationParts, double segmentationLength);

		public static void setSegmentation(Int64 model, Int64 segmentationParts, double segmentationLength)
		{
			if (IntPtr.Size == 4)
			{
				x86_setSegmentation((Int32)model, (Int32)segmentationParts, segmentationLength);
			}
			else
			{
				x64_setSegmentation(model, segmentationParts, segmentationLength);
			}
		}

		/// <summary>
		///		getSegmentation                                         (https://rdf.bg/ifcdoc/CS64/getSegmentation.html)
		///
		///	This returns the set values for segmentationParts and segmentationLength. Both attributes are optional.
		///	The values can be changed through the API call setSegmentation().
		///	The default values are
		///		segmentationParts  = 36
		///		segmentationLength = 0.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getSegmentation")]
		public static extern void x86_getSegmentation(Int32 model, out Int32 segmentationParts, out double segmentationLength);

		[DllImport(IFCEngineDLL, EntryPoint = "getSegmentation")]
		public static extern void x64_getSegmentation(Int64 model, out Int64 segmentationParts, out double segmentationLength);

		public static void getSegmentation(Int64 model, out Int64 segmentationParts, out double segmentationLength)
		{
			if (IntPtr.Size == 4)
			{
				x86_getSegmentation((Int32)model, out Int32 _segmentationParts, out double _segmentationLength);
				segmentationParts = _segmentationParts;
				segmentationLength = _segmentationLength;
			}
			else
			{
				x64_getSegmentation(model, out segmentationParts, out segmentationLength);
			}
		}

		/// <summary>
		///		setEpsilon                                              (https://rdf.bg/ifcdoc/CS64/setEpsilon.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "setEpsilon")]
		public static extern void x86_setEpsilon(Int32 model, Int32 mask, double absoluteEpsilon, double relativeEpsilon);

		[DllImport(IFCEngineDLL, EntryPoint = "setEpsilon")]
		public static extern void x64_setEpsilon(Int64 model, Int64 mask, double absoluteEpsilon, double relativeEpsilon);

		public static void setEpsilon(Int64 model, Int64 mask, double absoluteEpsilon, double relativeEpsilon)
		{
			if (IntPtr.Size == 4)
			{
				x86_setEpsilon((Int32)model, (Int32)mask, absoluteEpsilon, relativeEpsilon);
			}
			else
			{
				x64_setEpsilon(model, mask, absoluteEpsilon, relativeEpsilon);
			}
		}

		/// <summary>
		///		getEpsilon                                              (https://rdf.bg/ifcdoc/CS64/getEpsilon.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getEpsilon")]
		public static extern Int32 x86_getEpsilon(Int32 model, Int32 mask, out double absoluteEpsilon, out double relativeEpsilon);

		[DllImport(IFCEngineDLL, EntryPoint = "getEpsilon")]
		public static extern Int64 x64_getEpsilon(Int64 model, Int64 mask, out double absoluteEpsilon, out double relativeEpsilon);

		public static Int64 getEpsilon(Int64 model, Int64 mask, out double absoluteEpsilon, out double relativeEpsilon)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getEpsilon((Int32)model, (Int32)mask, out double _absoluteEpsilon, out double _relativeEpsilon);
				absoluteEpsilon = _absoluteEpsilon;
				relativeEpsilon = _relativeEpsilon;
				return _result;
			}
			else
			{
				return x64_getEpsilon(model, mask, out absoluteEpsilon, out relativeEpsilon);
			}
		}

        //
        //  Controling API Calls
        //

		/// <summary>
		///		circleSegments                                          (https://rdf.bg/ifcdoc/CS64/circleSegments.html)
		///
		///	Please use the setSegmentation call, note it is now a call that is model dependent.
		///
		///	The circleSegments(circles, smallCircles) can be replaced with
		///		double	segmentationLength = 0.;
		///		getSegmentation(model, nullptr, &segmentationLength);
		///		setSegmentation(model, circles, segmentationLength);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "circleSegments")]
		public static extern void x86_circleSegments(Int32 circles, Int32 smallCircles);

		[DllImport(IFCEngineDLL, EntryPoint = "circleSegments")]
		public static extern void x64_circleSegments(Int64 circles, Int64 smallCircles);

		public static void circleSegments(Int64 circles, Int64 smallCircles)
		{
			if (IntPtr.Size == 4)
			{
				x86_circleSegments((Int32)circles, (Int32)smallCircles);
			}
			else
			{
				x64_circleSegments(circles, smallCircles);
			}
		}

		/// <summary>
		///		setMaximumSegmentationLength                            (https://rdf.bg/ifcdoc/CS64/setMaximumSegmentationLength.html)
		///
		///	Please use setSegmentation call
		///
		///	The call setMaximumSegmentationLength(model, length) can be replaced with
		///		int_t segmentationParts = 0;
		///		getSegmentation(model, &segmentationParts, nullptr);
		///		setSegmentation(model, segmentationParts, length);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "setMaximumSegmentationLength")]
		public static extern void x86_setMaximumSegmentationLength(Int32 model, double length);

		[DllImport(IFCEngineDLL, EntryPoint = "setMaximumSegmentationLength")]
		public static extern void x64_setMaximumSegmentationLength(Int64 model, double length);

		public static void setMaximumSegmentationLength(Int64 model, double length)
		{
			if (IntPtr.Size == 4)
			{
				x86_setMaximumSegmentationLength((Int32)model, length);
			}
			else
			{
				x64_setMaximumSegmentationLength(model, length);
			}
		}

		/// <summary>
		///		getProjectUnitConversionFactor                          (https://rdf.bg/ifcdoc/CS64/getProjectUnitConversionFactor.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getProjectUnitConversionFactor")]
		public static extern double x86_getProjectUnitConversionFactor(Int32 model, string unitType, out IntPtr unitPrefix, out IntPtr unitName, out IntPtr SIUnitName);

		[DllImport(IFCEngineDLL, EntryPoint = "getProjectUnitConversionFactor")]
		public static extern double x64_getProjectUnitConversionFactor(Int64 model, string unitType, out IntPtr unitPrefix, out IntPtr unitName, out IntPtr SIUnitName);

		public static double getProjectUnitConversionFactor(Int64 model, string unitType, out IntPtr unitPrefix, out IntPtr unitName, out IntPtr SIUnitName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getProjectUnitConversionFactor((Int32)model, unitType, out IntPtr _unitPrefix, out IntPtr _unitName, out IntPtr _SIUnitName);
				unitPrefix = _unitPrefix;
				unitName = _unitName;
				SIUnitName = _SIUnitName;
				return _result;
			}
			else
			{
				return x64_getProjectUnitConversionFactor(model, unitType, out unitPrefix, out unitName, out SIUnitName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "getProjectUnitConversionFactor")]
		public static extern double x86_getProjectUnitConversionFactor(Int32 model, byte[] unitType, out IntPtr unitPrefix, out IntPtr unitName, out IntPtr SIUnitName);

		[DllImport(IFCEngineDLL, EntryPoint = "getProjectUnitConversionFactor")]
		public static extern double x64_getProjectUnitConversionFactor(Int64 model, byte[] unitType, out IntPtr unitPrefix, out IntPtr unitName, out IntPtr SIUnitName);

		public static double getProjectUnitConversionFactor(Int64 model, byte[] unitType, out IntPtr unitPrefix, out IntPtr unitName, out IntPtr SIUnitName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getProjectUnitConversionFactor((Int32)model, unitType, out IntPtr _unitPrefix, out IntPtr _unitName, out IntPtr _SIUnitName);
				unitPrefix = _unitPrefix;
				unitName = _unitName;
				SIUnitName = _SIUnitName;
				return _result;
			}
			else
			{
				return x64_getProjectUnitConversionFactor(model, unitType, out unitPrefix, out unitName, out SIUnitName);
			}
		}

		/// <summary>
		///		getUnitInstanceConversionFactor                         (https://rdf.bg/ifcdoc/CS64/getUnitInstanceConversionFactor.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getUnitInstanceConversionFactor")]
		public static extern double x86_getUnitInstanceConversionFactor(Int32 unitInstance, out IntPtr unitPrefix, out IntPtr unitName, out IntPtr SIUnitName);

		[DllImport(IFCEngineDLL, EntryPoint = "getUnitInstanceConversionFactor")]
		public static extern double x64_getUnitInstanceConversionFactor(Int64 unitInstance, out IntPtr unitPrefix, out IntPtr unitName, out IntPtr SIUnitName);

		public static double getUnitInstanceConversionFactor(Int64 unitInstance, out IntPtr unitPrefix, out IntPtr unitName, out IntPtr SIUnitName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getUnitInstanceConversionFactor((Int32)unitInstance, out IntPtr _unitPrefix, out IntPtr _unitName, out IntPtr _SIUnitName);
				unitPrefix = _unitPrefix;
				unitName = _unitName;
				SIUnitName = _SIUnitName;
				return _result;
			}
			else
			{
				return x64_getUnitInstanceConversionFactor(unitInstance, out unitPrefix, out unitName, out SIUnitName);
			}
		}

		/// <summary>
		///		setBRepProperties                                       (https://rdf.bg/ifcdoc/CS64/setBRepProperties.html)
		///
		///	This call can be used to optimize Boundary Representation geometries.
		///
		///		consistencyCheck
		///			bit0  (1)		merge elements in the vertex array are duplicated (epsilon used as distance)
		///			bit1  (2)		remove elements in the vertex array that are not referenced by elements in the index array (interpreted as SET if flags are defined)
		///			bit2  (4)		merge polygons placed in the same plane and sharing at least one edge
		///			bit3  (8)		merge polygons advanced (check of polygons have the opposite direction and are overlapping, but don't share points)
		///			bit4  (16)		check if faces are wrongly turned opposite from each other
		///			bit5  (32)		check if faces are inside-out
		///			bit6  (64)		check if faces result in solid, if not generate both sided faces
		///			bit7  (128)		invert direction of the face/normal information
		///			bit8  (256)		export all faces as one conceptual face
		///			bit9  (512)		remove irrelevant intermediate points on lines
		///			bit10 (1024)	check and repair faces that are not defined in a perfect plane
		///
		///		fraction
		///			To compare adjacent faces, they will be defined as being part of the same conceptual face if the fraction
		///			value is larger then the dot product of the normal vector's of the individual faces.
		///
		///		epsilon
		///			This value is used to compare vertex elements, if vertex elements should be merged and the distance is smaller than this epsilon value
		///			then it will be defined as equal
		///
		///		maxVerticesSize
		///			if 0 this setting is applied to BoundaryRepresentation based geometries
		///			if larger than 0 it is applied to all BoundaryRepresentation based geometries with vertices size smaller or equal to the given number
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "setBRepProperties")]
		public static extern void x86_setBRepProperties(Int32 model, Int64 consistencyCheck, double fraction, double epsilon, Int32 maxVerticesSize);

		[DllImport(IFCEngineDLL, EntryPoint = "setBRepProperties")]
		public static extern void x64_setBRepProperties(Int64 model, Int64 consistencyCheck, double fraction, double epsilon, Int64 maxVerticesSize);

		public static void setBRepProperties(Int64 model, Int64 consistencyCheck, double fraction, double epsilon, Int64 maxVerticesSize)
		{
			if (IntPtr.Size == 4)
			{
				x86_setBRepProperties((Int32)model, consistencyCheck, fraction, epsilon, (Int32)maxVerticesSize);
			}
			else
			{
				x64_setBRepProperties(model, consistencyCheck, fraction, epsilon, maxVerticesSize);
			}
		}

		/// <summary>
		///		cleanMemory                                             (https://rdf.bg/ifcdoc/CS64/cleanMemory.html)
		///
		///	This call forces cleaning of memory allocated.
		///	The following mode values are effected:
		///		0	non-cached geometry tree structures
		///		1	cached and non-cached geometry tree structures + resetting buffers for internally used Geometry Kernel instance
		///		3	cached and non-cached geometry tree structures
		///		4	clean memory allocated within a session for ADB structures and string values (including enumerations requested as wide char).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "cleanMemory")]
		public static extern void x86_cleanMemory(Int32 model, Int32 mode);

		[DllImport(IFCEngineDLL, EntryPoint = "cleanMemory")]
		public static extern void x64_cleanMemory(Int64 model, Int64 mode);

		public static void cleanMemory(Int64 model, Int64 mode)
		{
			if (IntPtr.Size == 4)
			{
				x86_cleanMemory((Int32)model, (Int32)mode);
			}
			else
			{
				x64_cleanMemory(model, mode);
			}
		}

		/// <summary>
		///		internalGetP21Line                                      (https://rdf.bg/ifcdoc/CS64/internalGetP21Line.html)
		///
		///	Returns the line STEP/Express ID of an instance
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "internalGetP21Line")]
		public static extern Int64 x86_internalGetP21Line(Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "internalGetP21Line")]
		public static extern Int64 x64_internalGetP21Line(Int64 instance);

		public static Int64 internalGetP21Line(Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_internalGetP21Line((Int32)instance);
				return _result;
			}
			else
			{
				return x64_internalGetP21Line(instance);
			}
		}

		/// <summary>
		///		internalForceInstanceFromP21Line                        (https://rdf.bg/ifcdoc/CS64/internalForceInstanceFromP21Line.html)
		///
		///	Returns an instance based on the model and STEP/Express ID (even when the instance itself might be non-existant).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "internalForceInstanceFromP21Line")]
		public static extern Int32 x86_internalForceInstanceFromP21Line(Int32 model, Int64 P21Line);

		[DllImport(IFCEngineDLL, EntryPoint = "internalForceInstanceFromP21Line")]
		public static extern Int64 x64_internalForceInstanceFromP21Line(Int64 model, Int64 P21Line);

		public static Int64 internalForceInstanceFromP21Line(Int64 model, Int64 P21Line)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_internalForceInstanceFromP21Line((Int32)model, P21Line);
				return _result;
			}
			else
			{
				return x64_internalForceInstanceFromP21Line(model, P21Line);
			}
		}

		/// <summary>
		///		internalGetInstanceFromP21Line                          (https://rdf.bg/ifcdoc/CS64/internalGetInstanceFromP21Line.html)
		///
		///	Returns an instance based on the model and STEP/Express ID
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "internalGetInstanceFromP21Line")]
		public static extern Int32 x86_internalGetInstanceFromP21Line(Int32 model, Int64 P21Line);

		[DllImport(IFCEngineDLL, EntryPoint = "internalGetInstanceFromP21Line")]
		public static extern Int64 x64_internalGetInstanceFromP21Line(Int64 model, Int64 P21Line);

		public static Int64 internalGetInstanceFromP21Line(Int64 model, Int64 P21Line)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_internalGetInstanceFromP21Line((Int32)model, P21Line);
				return _result;
			}
			else
			{
				return x64_internalGetInstanceFromP21Line(model, P21Line);
			}
		}

		/// <summary>
		///		internalGetXMLID                                        (https://rdf.bg/ifcdoc/CS64/internalGetXMLID.html)
		///
		///	In case an XML file is loaded the XML ID values are kept in memory and can be retrieved through this API call.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "internalGetXMLID")]
		public static extern IntPtr x86_internalGetXMLID(Int32 instance, out IntPtr XMLID);

		[DllImport(IFCEngineDLL, EntryPoint = "internalGetXMLID")]
		public static extern IntPtr x64_internalGetXMLID(Int64 instance, out IntPtr XMLID);

		public static IntPtr internalGetXMLID(Int64 instance, out IntPtr XMLID)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_internalGetXMLID((Int32)instance, out IntPtr _XMLID);
				XMLID = _XMLID;
				return _result;
			}
			else
			{
				return x64_internalGetXMLID(instance, out XMLID);
			}
		}

		/// <summary>
		///		setStringUnicode                                        (https://rdf.bg/ifcdoc/CS64/setStringUnicode.html)
		///
		///	Set mode of interpretation for arguments of type char*
		///		0 - char* (default)
		///		1 - wchar_t*
		///		2 - char16_t*
		///		4 - char32_t*
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "setStringUnicode")]
		public static extern Int32 x86_setStringUnicode(Int32 unicode);

		[DllImport(IFCEngineDLL, EntryPoint = "setStringUnicode")]
		public static extern Int64 x64_setStringUnicode(Int64 unicode);

		public static Int64 setStringUnicode(Int64 unicode)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_setStringUnicode((Int32)unicode);
				return _result;
			}
			else
			{
				return x64_setStringUnicode(unicode);
			}
		}

		/// <summary>
		///		getStringUnicode                                        (https://rdf.bg/ifcdoc/CS64/getStringUnicode.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getStringUnicode")]
		public static extern Int32 x86_getStringUnicode();

		[DllImport(IFCEngineDLL, EntryPoint = "getStringUnicode")]
		public static extern Int64 x64_getStringUnicode();

		public static Int64 getStringUnicode()
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getStringUnicode();
				return _result;
			}
			else
			{
				return x64_getStringUnicode();
			}
		}

		/// <summary>
		///		engiSetStringEncoding                                   (https://rdf.bg/ifcdoc/CS64/engiSetStringEncoding.html)
		///
		///	Sets encoding for sdaiSTRING data type in put and get functions
		///	if model is NULL it will set codepage for models, created after the call or for contexts when model is not known
		///	returns 1 when successful of 0 when fails.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiSetStringEncoding")]
		public static extern Int32 x86_engiSetStringEncoding(Int32 model, byte encoding);

		[DllImport(IFCEngineDLL, EntryPoint = "engiSetStringEncoding")]
		public static extern Int64 x64_engiSetStringEncoding(Int64 model, byte encoding);

		public static Int64 engiSetStringEncoding(Int64 model, byte encoding)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiSetStringEncoding((Int32)model, encoding);
				return _result;
			}
			else
			{
				return x64_engiSetStringEncoding(model, encoding);
			}
		}

		/// <summary>
		///		setFilter                                               (https://rdf.bg/ifcdoc/CS64/setFilter.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "setFilter")]
		public static extern void x86_setFilter(Int32 model, Int32 setting, Int32 mask);

		[DllImport(IFCEngineDLL, EntryPoint = "setFilter")]
		public static extern void x64_setFilter(Int64 model, Int64 setting, Int64 mask);

		public static void setFilter(Int64 model, Int64 setting, Int64 mask)
		{
			if (IntPtr.Size == 4)
			{
				x86_setFilter((Int32)model, (Int32)setting, (Int32)mask);
			}
			else
			{
				x64_setFilter(model, setting, mask);
			}
		}

		/// <summary>
		///		getFilter                                               (https://rdf.bg/ifcdoc/CS64/getFilter.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getFilter")]
		public static extern Int32 x86_getFilter(Int32 model, Int32 mask);

		[DllImport(IFCEngineDLL, EntryPoint = "getFilter")]
		public static extern Int64 x64_getFilter(Int64 model, Int64 mask);

		public static Int64 getFilter(Int64 model, Int64 mask)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getFilter((Int32)model, (Int32)mask);
				return _result;
			}
			else
			{
				return x64_getFilter(model, mask);
			}
		}

		/// <summary>
		///		setSerialization                                        (https://rdf.bg/ifcdoc/CS64/setSerialization.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "setSerialization")]
		public static extern void x86_setSerialization(Int32 model, Int32 setting, Int32 mask);

		[DllImport(IFCEngineDLL, EntryPoint = "setSerialization")]
		public static extern void x64_setSerialization(Int64 model, Int64 setting, Int64 mask);

		public static void setSerialization(Int64 model, Int64 setting, Int64 mask)
		{
			if (IntPtr.Size == 4)
			{
				x86_setSerialization((Int32)model, (Int32)setting, (Int32)mask);
			}
			else
			{
				x64_setSerialization(model, setting, mask);
			}
		}

		/// <summary>
		///		getSerialization                                        (https://rdf.bg/ifcdoc/CS64/getSerialization.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getSerialization")]
		public static extern Int32 x86_getSerialization(Int32 model, Int32 mask);

		[DllImport(IFCEngineDLL, EntryPoint = "getSerialization")]
		public static extern Int64 x64_getSerialization(Int64 model, Int64 mask);

		public static Int64 getSerialization(Int64 model, Int64 mask)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getSerialization((Int32)model, (Int32)mask);
				return _result;
			}
			else
			{
				return x64_getSerialization(model, mask);
			}
		}

        //
        //  Uncategorized API Calls
        //

		/// <summary>
		///		xxxxGetEntityAndSubTypesExtent                          (https://rdf.bg/ifcdoc/CS64/xxxxGetEntityAndSubTypesExtent.html)
		///
		///	Model input parameter is irrelevant, but is required for backwards compatibility.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetEntityAndSubTypesExtent")]
		public static extern Int32 x86_xxxxGetEntityAndSubTypesExtent(Int32 model, Int32 entity);

		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetEntityAndSubTypesExtent")]
		public static extern Int64 x64_xxxxGetEntityAndSubTypesExtent(Int64 model, Int64 entity);

		public static Int64 xxxxGetEntityAndSubTypesExtent(Int64 model, Int64 entity)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_xxxxGetEntityAndSubTypesExtent((Int32)model, (Int32)entity);
				return _result;
			}
			else
			{
				return x64_xxxxGetEntityAndSubTypesExtent(model, entity);
			}
		}

		/// <summary>
		///		xxxxGetEntityAndSubTypesExtentBN                        (https://rdf.bg/ifcdoc/CS64/xxxxGetEntityAndSubTypesExtentBN.html)
		///
		///	Technically xxxxGetEntityAndSubTypesExtentBN will transform into the following call
		///		xxxxGetEntityAndSubTypesExtent(
		///				model,
		///				sdaiGetEntity(
		///						model,
		///						entityName
		///					)
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetEntityAndSubTypesExtentBN")]
		public static extern Int32 x86_xxxxGetEntityAndSubTypesExtentBN(Int32 model, string entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetEntityAndSubTypesExtentBN")]
		public static extern Int64 x64_xxxxGetEntityAndSubTypesExtentBN(Int64 model, string entityName);

		public static Int64 xxxxGetEntityAndSubTypesExtentBN(Int64 model, string entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_xxxxGetEntityAndSubTypesExtentBN((Int32)model, entityName);
				return _result;
			}
			else
			{
				return x64_xxxxGetEntityAndSubTypesExtentBN(model, entityName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetEntityAndSubTypesExtentBN")]
		public static extern Int32 x86_xxxxGetEntityAndSubTypesExtentBN(Int32 model, byte[] entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetEntityAndSubTypesExtentBN")]
		public static extern Int64 x64_xxxxGetEntityAndSubTypesExtentBN(Int64 model, byte[] entityName);

		public static Int64 xxxxGetEntityAndSubTypesExtentBN(Int64 model, byte[] entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_xxxxGetEntityAndSubTypesExtentBN((Int32)model, entityName);
				return _result;
			}
			else
			{
				return x64_xxxxGetEntityAndSubTypesExtentBN(model, entityName);
			}
		}

		/// <summary>
		///		xxxxGetAllInstances                                     (https://rdf.bg/ifcdoc/CS64/xxxxGetAllInstances.html)
		///
		///	This call returns an aggregation containing all instances.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetAllInstances")]
		public static extern Int32 x86_xxxxGetAllInstances(Int32 model);

		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetAllInstances")]
		public static extern Int64 x64_xxxxGetAllInstances(Int64 model);

		public static Int64 xxxxGetAllInstances(Int64 model)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_xxxxGetAllInstances((Int32)model);
				return _result;
			}
			else
			{
				return x64_xxxxGetAllInstances(model);
			}
		}

		/// <summary>
		///		xxxxGetInstancesUsing                                   (https://rdf.bg/ifcdoc/CS64/xxxxGetInstancesUsing.html)
		///
		///	This call returns an aggregation containing all instances referencing the given instance.
		///
		///	note: this is independent from if there are inverse relations defining such an aggregation or parts of it.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetInstancesUsing")]
		public static extern Int32 x86_xxxxGetInstancesUsing(Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetInstancesUsing")]
		public static extern Int64 x64_xxxxGetInstancesUsing(Int64 instance);

		public static Int64 xxxxGetInstancesUsing(Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_xxxxGetInstancesUsing((Int32)instance);
				return _result;
			}
			else
			{
				return x64_xxxxGetInstancesUsing(instance);
			}
		}

		/// <summary>
		///		xxxxDeleteFromAggregation                               (https://rdf.bg/ifcdoc/CS64/xxxxDeleteFromAggregation.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "xxxxDeleteFromAggregation")]
		public static extern Int32 x86_xxxxDeleteFromAggregation(Int32 instance, Int32 aggregate, Int32 elementIndex);

		[DllImport(IFCEngineDLL, EntryPoint = "xxxxDeleteFromAggregation")]
		public static extern Int64 x64_xxxxDeleteFromAggregation(Int64 instance, Int64 aggregate, Int64 elementIndex);

		public static Int64 xxxxDeleteFromAggregation(Int64 instance, Int64 aggregate, Int64 elementIndex)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_xxxxDeleteFromAggregation((Int32)instance, (Int32)aggregate, (Int32)elementIndex);
				return _result;
			}
			else
			{
				return x64_xxxxDeleteFromAggregation(instance, aggregate, elementIndex);
			}
		}

		/// <summary>
		///		xxxxGetAttrDefinitionByValue                            (https://rdf.bg/ifcdoc/CS64/xxxxGetAttrDefinitionByValue.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetAttrDefinitionByValue")]
		public static extern Int32 x86_xxxxGetAttrDefinitionByValue(Int32 instance, out IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetAttrDefinitionByValue")]
		public static extern Int64 x64_xxxxGetAttrDefinitionByValue(Int64 instance, out IntPtr value);

		public static Int64 xxxxGetAttrDefinitionByValue(Int64 instance, out IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_xxxxGetAttrDefinitionByValue((Int32)instance, out IntPtr _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_xxxxGetAttrDefinitionByValue(instance, out value);
			}
		}

		/// <summary>
		///		xxxxGetAttrNameByIndex                                  (https://rdf.bg/ifcdoc/CS64/xxxxGetAttrNameByIndex.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetAttrNameByIndex")]
		public static extern IntPtr x86_xxxxGetAttrNameByIndex(Int32 instance, Int32 index, out IntPtr name);

		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetAttrNameByIndex")]
		public static extern IntPtr x64_xxxxGetAttrNameByIndex(Int64 instance, Int64 index, out IntPtr name);

		public static IntPtr xxxxGetAttrNameByIndex(Int64 instance, Int64 index, out IntPtr name)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_xxxxGetAttrNameByIndex((Int32)instance, (Int32)index, out IntPtr _name);
				name = _name;
				return _result;
			}
			else
			{
				return x64_xxxxGetAttrNameByIndex(instance, index, out name);
			}
		}

		/// <summary>
		///		iterateOverInstances                                    (https://rdf.bg/ifcdoc/CS64/iterateOverInstances.html)
		///
		///	This function iterates over all available instances loaded in memory, it is the fastest way to find all instances.
		///	Argument entity and entityName are both optional and if non-zero are filled with respectively the entity handle and entity name as char array.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "iterateOverInstances")]
		public static extern Int32 x86_iterateOverInstances(Int32 model, Int32 instance, Int32 entity, out IntPtr entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "iterateOverInstances")]
		public static extern Int64 x64_iterateOverInstances(Int64 model, Int64 instance, Int64 entity, out IntPtr entityName);

		public static Int64 iterateOverInstances(Int64 model, Int64 instance, Int64 entity, out IntPtr entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_iterateOverInstances((Int32)model, (Int32)instance, (Int32)entity, out IntPtr _entityName);
				entityName = _entityName;
				return _result;
			}
			else
			{
				return x64_iterateOverInstances(model, instance, entity, out entityName);
			}
		}

		/// <summary>
		///		iterateOverProperties                                   (https://rdf.bg/ifcdoc/CS64/iterateOverProperties.html)
		///
		///	This function iterated over all available attributes of a specific given entity.
		///	This call is typically used in combination with iterateOverInstances(..).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "iterateOverProperties")]
		public static extern Int32 x86_iterateOverProperties(Int32 entity, Int32 index);

		[DllImport(IFCEngineDLL, EntryPoint = "iterateOverProperties")]
		public static extern Int64 x64_iterateOverProperties(Int64 entity, Int64 index);

		public static Int64 iterateOverProperties(Int64 entity, Int64 index)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_iterateOverProperties((Int32)entity, (Int32)index);
				return _result;
			}
			else
			{
				return x64_iterateOverProperties(entity, index);
			}
		}

		/// <summary>
		///		sdaiGetAggrByIterator                                   (https://rdf.bg/ifcdoc/CS64/sdaiGetAggrByIterator.html)
		///
		///	valueType argument to specify what type of data caller wants to get and
		///	value argument where the caller should provide a buffer, and the function will write the result to.
		///
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiGetAggrByIterator, and it works similarly for all get-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///	The Table 2 shows what valueType can be fulfilled depending on actual model data.
		///	If a get-function cannot get a value it will return 0, it may happen when model item is unset ($) or incompatible with requested valueType.
		///	To separate these cases you can use engiGetInstanceAttrType(BN), sdaiGetADBType and engiGetAggrType.
		///	On success get-function will return non-zero. More precisely, according to ISO 10303-24-2001 on success they return content of
		///	value argument (*value) for sdaiADB, sdaiAGGR, or sdaiINSTANCE or value argument itself for other types (it has no useful meaning for C#).
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiGetAggrByIterator but valid for all get-functions)
		///
		///	valueType				C/C++															C#
		///
		///	sdaiINTEGER				int_t val;														int_t val;
		///							sdaiGetAggrByIterator (iterator, sdaiINTEGER, &val);			ifcengine.sdaiGetAggrByIterator (iterator, ifcengine.sdaiINTEGER, out val);
		///
		///	sdaiREAL or sdaiNUMBER	double val;														double val;
		///							sdaiGetAggrByIterator (iterator, sdaiREAL, &val);				ifcengine.sdaiGetAggrByIterator (iterator, ifcengine.sdaiREAL, out val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val;												bool val;
		///							sdaiGetAggrByIterator (iterator, sdaiBOOLEAN, &val);			ifcengine.sdaiGetAggrByIterator (iterator, ifcengine.sdaiBOOLEAN, out val);
		///
		///	sdaiLOGICAL				const TCHAR* val;												string val;
		///							sdaiGetAggrByIterator (iterator, sdaiLOGICAL, &val);			ifcengine.sdaiGetAggrByIterator (iterator, ifcengine.sdaiLOGICAL, out val);
		///
		///	sdaiENUM				const TCHAR* val;												string val;
		///							sdaiGetAggrByIterator (iterator, sdaiENUM, &val);				ifcengine.sdaiGetAggrByIterator (iterator, ifcengine.sdaiENUM, out val);
		///
		///	sdaiBINARY				const TCHAR* val;												string val;
		///							sdaiGetAggrByIterator (iterator, sdaiBINARY, &val);				ifcengine.sdaiGetAggrByIterator (iterator, ifcengine.sdaiBINARY, out val);
		///
		///	sdaiSTRING				const char* val;												string val;
		///							sdaiGetAggrByIterator (iterator, sdaiSTRING, &val);				ifcengine.sdaiGetAggrByIterator (iterator, ifcengine.sdaiSTRING, out val);
		///
		///	sdaiUNICODE				const wchar_t* val;												string val;
		///							sdaiGetAggrByIterator (iterator, sdaiUNICODE, &val);			ifcengine.sdaiGetAggrByIterator (iterator, ifcengine.sdaiUNICODE, out val);
		///
		///	sdaiEXPRESSSTRING		const char* val;												string val;
		///							sdaiGetAggrByIterator (iterator, sdaiEXPRESSSTRING, &val);		ifcengine.sdaiGetAggrByIterator (iterator, ifcengine.sdaiEXPRESSSTRING, out val);
		///
		///	sdaiINSTANCE			SdaiInstance val;												int_t val;
		///							sdaiGetAggrByIterator (iterator, sdaiINSTANCE, &val);			ifcengine.sdaiGetAggrByIterator (iterator, ifcengine.sdaiINSTANCE, out val);
		///
		///	sdaiAGGR				SdaiAggr aggr;													int_t aggr;
		///							sdaiGetAggrByIterator (iterator, sdaiAGGR, &aggr);				ifcengine.sdaiGetAggrByIterator (iterator, ifcengine.sdaiAGGR, out aggr);
		///
		///	sdaiADB					SdaiADB adb = sdaiCreateEmptyADB();								int_t adb = 0;	//	it is important to initialize
		///							sdaiGetAggrByIterator (iterator, sdaiADB, adb);					ifcengine.sdaiGetAggrByIterator (iterator, ifcengine.sdaiADB, out adb);		
		///							sdaiDeleteADB (adb);
		///
		///							SdaiADB adb = nullptr;	//	it is important to initialize
		///							sdaiGetAggrByIterator (iterator, sdaiADB, &adb);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///	(Non-standard extension) sdiADB in C++ has an option to work without sdaiCreateEmptyADB and sdaiDeleteADB as shown in the table.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			Yes *		 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			Yes			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			Yes			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .			 .			 .
		///	sdaiUNICODE			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		///	Note: sdaiGetAttr, stdaiGetAttrBN, engiGetElement will success with any model data, except non-set($)
		///		  (Non-standard extensions) sdaiGetADBValue: sdaiADB is allowed and will success when sdaiGetADBTypePath is not NULL, returning ABD value has type path element removed.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrByIterator")]
		public static extern Int32 x86_sdaiGetAggrByIterator(Int32 iterator, Int32 valueType, out bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrByIterator")]
		public static extern Int64 x64_sdaiGetAggrByIterator(Int64 iterator, Int64 valueType, out bool value);

		public static Int64 sdaiGetAggrByIterator(Int64 iterator, Int64 valueType, out bool value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAggrByIterator((Int32)iterator, (Int32)valueType, out bool _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAggrByIterator(iterator, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrByIterator")]
		public static extern Int32 x86_sdaiGetAggrByIterator(Int32 iterator, Int32 valueType, out Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrByIterator")]
		public static extern Int64 x64_sdaiGetAggrByIterator(Int64 iterator, Int64 valueType, out Int64 value);

		public static Int64 sdaiGetAggrByIterator(Int64 iterator, Int64 valueType, out Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAggrByIterator((Int32)iterator, (Int32)valueType, out Int32 _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAggrByIterator(iterator, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrByIterator")]
		public static extern Int32 x86_sdaiGetAggrByIterator(Int32 iterator, Int32 valueType, out double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrByIterator")]
		public static extern Int64 x64_sdaiGetAggrByIterator(Int64 iterator, Int64 valueType, out double value);

		public static Int64 sdaiGetAggrByIterator(Int64 iterator, Int64 valueType, out double value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAggrByIterator((Int32)iterator, (Int32)valueType, out double _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAggrByIterator(iterator, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrByIterator")]
		public static extern Int32 x86_sdaiGetAggrByIterator(Int32 iterator, Int32 valueType, out IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetAggrByIterator")]
		public static extern Int64 x64_sdaiGetAggrByIterator(Int64 iterator, Int64 valueType, out IntPtr value);

		public static Int64 sdaiGetAggrByIterator(Int64 iterator, Int64 valueType, out IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetAggrByIterator((Int32)iterator, (Int32)valueType, out IntPtr _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_sdaiGetAggrByIterator(iterator, valueType, out value);
			}
		}

		/// <summary>
		///		sdaiPutAggrByIterator                                   (https://rdf.bg/ifcdoc/CS64/sdaiPutAggrByIterator.html)
		///
		///	valueType argument to specify what type of data caller wants to put
		///	Table 1 shows type of buffer the caller should provide depending on the valueType for sdaiPutAggrByIterator, and it works similarly for all put-functions.
		///	Note: with SDAI API it is impossible to check buffer type at compilation or execution time and this is responsibility of a caller to ensure that
		///		  requested valueType is matching with the value argument, a mismatch will lead to unpredictable results.
		///
		///
		///	Table 1 – Required value buffer depending on valueType (on the example of sdaiPutAggrByIterator but valid for all put-functions)
		///
		///	valueType				C/C++														C#
		///
		///	sdaiINTEGER				int_t val = 123;											int_t val = 123;
		///							sdaiPutAggrByIterator (iterator, sdaiINTEGER, &val);		ifcengine.sdaiPutAggrByIterator (iterator, ifcengine.sdaiINTEGER, ref val);
		///
		///	sdaiREAL or sdaiNUMBER	double val = 123.456;										double val = 123.456;
		///							sdaiPutAggrByIterator (iterator, sdaiREAL, &val);			ifcengine.sdaiPutAggrByIterator (iterator, ifcengine.sdaiREAL, ref val);
		///
		///	sdaiBOOLEAN				SdaiBoolean val = sdaiTRUE;									bool val = true;
		///							sdaiPutAggrByIterator (iterator, sdaiBOOLEAN, &val);		ifcengine.sdaiPutAggrByIterator (iterator, ifcengine.sdaiBOOLEAN, ref val);
		///
		///	sdaiLOGICAL				const TCHAR* val = "U";										string val = "U";
		///							sdaiPutAggrByIterator (iterator, sdaiLOGICAL, val);			ifcengine.sdaiPutAggrByIterator (iterator, ifcengine.sdaiLOGICAL, val);
		///
		///	sdaiENUM				const TCHAR* val = "NOTDEFINED";							string val = "NOTDEFINED";
		///							sdaiPutAggrByIterator (iterator, sdaiENUM, val);			ifcengine.sdaiPutAggrByIterator (iterator, ifcengine.sdaiENUM, val);
		///
		///	sdaiBINARY				const TCHAR* val = "0123456ABC";							string val = "0123456ABC";
		///							sdaiPutAggrByIterator (iterator, sdaiBINARY, val);			ifcengine.sdaiPutAggrByIterator (iterator, ifcengine.sdaiBINARY, val);
		///
		///	sdaiSTRING				const char* val = "My Simple String";						string val = "My Simple String";
		///							sdaiPutAggrByIterator (iterator, sdaiSTRING, val);			ifcengine.sdaiPutAggrByIterator (iterator, ifcengine.sdaiSTRING, val);
		///
		///	sdaiUNICODE				const wchar_t* val = L"Any Unicode String";					string val = "Any Unicode String";
		///							sdaiPutAggrByIterator (iterator, sdaiUNICODE, val);			ifcengine.sdaiPutAggrByIterator (iterator, ifcengine.sdaiUNICODE, val);
		///
		///	sdaiEXPRESSSTRING		const char* val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";	string val = "EXPRESS format, i.e. \\X2\\00FC\\X0\\";
		///							sdaiPutAggrByIterator (iterator, sdaiEXPRESSSTRING, val);	ifcengine.sdaiPutAggrByIterator (iterator, ifcengine.sdaiEXPRESSSTRING, val);
		///
		///	sdaiINSTANCE			SdaiInstance val = sdaiCreateInstanceBN (model, "IFCSITE");	int_t val = ifcengine.sdaiCreateInstanceBN (model, "IFCSITE");
		///							sdaiPutAggrByIterator (iterator, sdaiINSTANCE, val);		ifcengine.sdaiPutAggrByIterator (iterator, ifcengine.sdaiINSTANCE, val);
		///
		///	sdaiAGGR				SdaiAggr val = sdaiCreateAggr (inst, 0);					int_t val = sdaiCreateAggr (inst, 0);
		///							sdaiPutAttr (val, sdaiINSTANCE, inst);						ifcengine.sdaiPutAttr (val, ifcengine.sdaiINSTANCE, inst);
		///							sdaiPutAggrByIterator (iterator, sdaiAGGR, val);			ifcengine.sdaiPutAggrByIterator (iterator, ifcengine.sdaiAGGR, val);
		///
		///	sdaiADB					int_t integerValue = 123;									int_t integerValue = 123;	
		///							SdaiADB val = sdaiCreateADB (sdaiINTEGER, &integerValue);	int_t val = ifcengine.sdaiCreateADB (ifcengine.sdaiINTEGER, ref integerValue);
		///							sdaiPutADBTypePath (val, 1, "IFCINTEGER");					ifcengine.sdaiPutADBTypePath (val, 1, "IFCINTEGER");
		///							sdaiPutAggrByIterator (iterator, sdaiADB, val);				ifcengine.sdaiPutAggrByIterator (iterator, ifcengine.sdaiADB, val);	
		///							sdaiDeleteADB (val);										ifcengine.sdaiDeleteADB (val);
		///
		///	TCHAR is “char” or “wchar_t” depending on setStringUnicode.
		///	(Non-standard behavior) sdaiLOGICAL behaves differently from ISO 10303-24-2001: it expects char* while standard declares int_t.
		///	(Non-standard extension) sdiADB in C++ has an option to work without sdaiCreateEmptyADB and sdaiDeleteADB as shown in the table.
		///
		///
		///	Table 2 - valueType can be requested depending on actual model data.
		///
		///	valueType		Works for following values in the model
		///				 	  integer	   real		.T. or .F.	   .U.		other enum	  binary	  string	 instance	   list		 $ (empty)
		///	sdaiINTEGER			Yes			 .			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiREAL			 .			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiNUMBER			 . 			Yes			 .			 .			 .			 .			 .			 .			 .			 .
		///	sdaiBOOLEAN			 .			 .			Yes			 .			 .			 .			 .			 .			 .			 .
		///	sdaiLOGICAL			 .			 .			Yes			Yes			 .			 .			 .			 .			 .			 .
		///	sdaiENUM			 .			 .			Yes			Yes			Yes			 .			 .			 .			 .			 .
		///	sdaiBINARY			 .			 .			 .			 .			 .			Yes			 .			 .			 .			 .
		///	sdaiSTRING			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiUNICODE			 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiEXPRESSSTRING	 .			 .			 .			 .			 .			 .			Yes			 .			 .			 .
		///	sdaiINSTANCE		 .			 .			 .			 .			 .			 .			 .			Yes			 .			 .
		///	sdaiAGGR			 .			 .			 .			 .			 .			 .			 .			 .			Yes			 .
		///	sdaiADB				Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			Yes			 .
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIterator")]
		public static extern void x86_sdaiPutAggrByIterator(Int32 iterator, Int32 valueType, ref bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIterator")]
		public static extern void x64_sdaiPutAggrByIterator(Int64 iterator, Int64 valueType, ref bool value);

		public static void sdaiPutAggrByIterator(Int64 iterator, Int64 valueType, ref bool value)
		{
			if (IntPtr.Size == 4)
			{
				bool _value = (bool)value;
				x86_sdaiPutAggrByIterator((Int32)iterator, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAggrByIterator(iterator, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIterator")]
		public static extern void x86_sdaiPutAggrByIterator(Int32 iterator, Int32 valueType, ref Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIterator")]
		public static extern void x64_sdaiPutAggrByIterator(Int64 iterator, Int64 valueType, ref Int64 value);

		public static void sdaiPutAggrByIterator(Int64 iterator, Int64 valueType, ref Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				Int32 _value = (Int32)value;
				x86_sdaiPutAggrByIterator((Int32)iterator, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAggrByIterator(iterator, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIterator")]
		public static extern void x86_sdaiPutAggrByIterator(Int32 iterator, Int32 valueType, Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIterator")]
		public static extern void x64_sdaiPutAggrByIterator(Int64 iterator, Int64 valueType, Int64 value);

		public static void sdaiPutAggrByIterator(Int64 iterator, Int64 valueType, Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutAggrByIterator((Int32)iterator, (Int32)valueType, (Int32)value);
			}
			else
			{
				x64_sdaiPutAggrByIterator(iterator, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIterator")]
		public static extern void x86_sdaiPutAggrByIterator(Int32 iterator, Int32 valueType, ref double value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIterator")]
		public static extern void x64_sdaiPutAggrByIterator(Int64 iterator, Int64 valueType, ref double value);

		public static void sdaiPutAggrByIterator(Int64 iterator, Int64 valueType, ref double value)
		{
			if (IntPtr.Size == 4)
			{
				double _value = (double)value;
				x86_sdaiPutAggrByIterator((Int32)iterator, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAggrByIterator(iterator, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIterator")]
		public static extern void x86_sdaiPutAggrByIterator(Int32 iterator, Int32 valueType, ref IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIterator")]
		public static extern void x64_sdaiPutAggrByIterator(Int64 iterator, Int64 valueType, ref IntPtr value);

		public static void sdaiPutAggrByIterator(Int64 iterator, Int64 valueType, ref IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				IntPtr _value = (IntPtr)value;
				x86_sdaiPutAggrByIterator((Int32)iterator, (Int32)valueType, ref _value);
				value = _value;
			}
			else
			{
				x64_sdaiPutAggrByIterator(iterator, valueType, ref value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIterator")]
		public static extern void x86_sdaiPutAggrByIterator(Int32 iterator, Int32 valueType, byte[] value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIterator")]
		public static extern void x64_sdaiPutAggrByIterator(Int64 iterator, Int64 valueType, byte[] value);

		public static void sdaiPutAggrByIterator(Int64 iterator, Int64 valueType, byte[] value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutAggrByIterator((Int32)iterator, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiPutAggrByIterator(iterator, valueType, value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIterator")]
		public static extern void x86_sdaiPutAggrByIterator(Int32 iterator, Int32 valueType, string value);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiPutAggrByIterator")]
		public static extern void x64_sdaiPutAggrByIterator(Int64 iterator, Int64 valueType, string value);

		public static void sdaiPutAggrByIterator(Int64 iterator, Int64 valueType, string value)
		{
			if (IntPtr.Size == 4)
			{
				x86_sdaiPutAggrByIterator((Int32)iterator, (Int32)valueType, value);
			}
			else
			{
				x64_sdaiPutAggrByIterator(iterator, valueType, value);
			}
		}

		/// <summary>
		///		internalSetLink                                         (https://rdf.bg/ifcdoc/CS64/internalSetLink.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "internalSetLink")]
		public static extern void x86_internalSetLink(Int32 instance, string attributeName, Int32 linked_id);

		[DllImport(IFCEngineDLL, EntryPoint = "internalSetLink")]
		public static extern void x64_internalSetLink(Int64 instance, string attributeName, Int64 linked_id);

		public static void internalSetLink(Int64 instance, string attributeName, Int64 linked_id)
		{
			if (IntPtr.Size == 4)
			{
				x86_internalSetLink((Int32)instance, attributeName, (Int32)linked_id);
			}
			else
			{
				x64_internalSetLink(instance, attributeName, linked_id);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "internalSetLink")]
		public static extern void x86_internalSetLink(Int32 instance, byte[] attributeName, Int32 linked_id);

		[DllImport(IFCEngineDLL, EntryPoint = "internalSetLink")]
		public static extern void x64_internalSetLink(Int64 instance, byte[] attributeName, Int64 linked_id);

		public static void internalSetLink(Int64 instance, byte[] attributeName, Int64 linked_id)
		{
			if (IntPtr.Size == 4)
			{
				x86_internalSetLink((Int32)instance, attributeName, (Int32)linked_id);
			}
			else
			{
				x64_internalSetLink(instance, attributeName, linked_id);
			}
		}

		/// <summary>
		///		internalAddAggrLink                                     (https://rdf.bg/ifcdoc/CS64/internalAddAggrLink.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "internalAddAggrLink")]
		public static extern void x86_internalAddAggrLink(Int32 aggregate, Int32 linked_id);

		[DllImport(IFCEngineDLL, EntryPoint = "internalAddAggrLink")]
		public static extern void x64_internalAddAggrLink(Int64 aggregate, Int64 linked_id);

		public static void internalAddAggrLink(Int64 aggregate, Int64 linked_id)
		{
			if (IntPtr.Size == 4)
			{
				x86_internalAddAggrLink((Int32)aggregate, (Int32)linked_id);
			}
			else
			{
				x64_internalAddAggrLink(aggregate, linked_id);
			}
		}

		/// <summary>
		///		engiGetNotReferedAggr                                   (https://rdf.bg/ifcdoc/CS64/engiGetNotReferedAggr.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetNotReferedAggr")]
		public static extern void x86_engiGetNotReferedAggr(Int32 model, out Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetNotReferedAggr")]
		public static extern void x64_engiGetNotReferedAggr(Int64 model, out Int64 value);

		public static void engiGetNotReferedAggr(Int64 model, out Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiGetNotReferedAggr((Int32)model, out Int32 _value);
				value = _value;
			}
			else
			{
				x64_engiGetNotReferedAggr(model, out value);
			}
		}

		/// <summary>
		///		engiGetAttributeAggr                                    (https://rdf.bg/ifcdoc/CS64/engiGetAttributeAggr.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttributeAggr")]
		public static extern void x86_engiGetAttributeAggr(Int32 instance, out Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttributeAggr")]
		public static extern void x64_engiGetAttributeAggr(Int64 instance, out Int64 value);

		public static void engiGetAttributeAggr(Int64 instance, out Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiGetAttributeAggr((Int32)instance, out Int32 _value);
				value = _value;
			}
			else
			{
				x64_engiGetAttributeAggr(instance, out value);
			}
		}

		/// <summary>
		///		engiGetAggrUnknownElement                               (https://rdf.bg/ifcdoc/CS64/engiGetAggrUnknownElement.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrUnknownElement")]
		public static extern void x86_engiGetAggrUnknownElement(Int32 aggregate, Int32 elementIndex, out Int32 valueType, out bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrUnknownElement")]
		public static extern void x64_engiGetAggrUnknownElement(Int64 aggregate, Int64 elementIndex, out Int64 valueType, out bool value);

		public static void engiGetAggrUnknownElement(Int64 aggregate, Int64 elementIndex, out Int64 valueType, out bool value)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiGetAggrUnknownElement((Int32)aggregate, (Int32)elementIndex, out Int32 _valueType, out bool _value);
				valueType = _valueType;
				value = _value;
			}
			else
			{
				x64_engiGetAggrUnknownElement(aggregate, elementIndex, out valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrUnknownElement")]
		public static extern void x86_engiGetAggrUnknownElement(Int32 aggregate, Int32 elementIndex, out Int32 valueType, out Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrUnknownElement")]
		public static extern void x64_engiGetAggrUnknownElement(Int64 aggregate, Int64 elementIndex, out Int64 valueType, out Int64 value);

		public static void engiGetAggrUnknownElement(Int64 aggregate, Int64 elementIndex, out Int64 valueType, out Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiGetAggrUnknownElement((Int32)aggregate, (Int32)elementIndex, out Int32 _valueType, out Int32 _value);
				valueType = _valueType;
				value = _value;
			}
			else
			{
				x64_engiGetAggrUnknownElement(aggregate, elementIndex, out valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrUnknownElement")]
		public static extern void x86_engiGetAggrUnknownElement(Int32 aggregate, Int32 elementIndex, out Int32 valueType, out double value);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrUnknownElement")]
		public static extern void x64_engiGetAggrUnknownElement(Int64 aggregate, Int64 elementIndex, out Int64 valueType, out double value);

		public static void engiGetAggrUnknownElement(Int64 aggregate, Int64 elementIndex, out Int64 valueType, out double value)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiGetAggrUnknownElement((Int32)aggregate, (Int32)elementIndex, out Int32 _valueType, out double _value);
				valueType = _valueType;
				value = _value;
			}
			else
			{
				x64_engiGetAggrUnknownElement(aggregate, elementIndex, out valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrUnknownElement")]
		public static extern void x86_engiGetAggrUnknownElement(Int32 aggregate, Int32 elementIndex, out Int32 valueType, out IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrUnknownElement")]
		public static extern void x64_engiGetAggrUnknownElement(Int64 aggregate, Int64 elementIndex, out Int64 valueType, out IntPtr value);

		public static void engiGetAggrUnknownElement(Int64 aggregate, Int64 elementIndex, out Int64 valueType, out IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiGetAggrUnknownElement((Int32)aggregate, (Int32)elementIndex, out Int32 _valueType, out IntPtr _value);
				valueType = _valueType;
				value = _value;
			}
			else
			{
				x64_engiGetAggrUnknownElement(aggregate, elementIndex, out valueType, out value);
			}
		}

		/// <summary>
		///		sdaiErrorQuery                                          (https://rdf.bg/ifcdoc/CS64/sdaiErrorQuery.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiErrorQuery")]
		public static extern Int32 x86_sdaiErrorQuery();

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiErrorQuery")]
		public static extern Int64 x64_sdaiErrorQuery();

		public static Int64 sdaiErrorQuery()
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiErrorQuery();
				return _result;
			}
			else
			{
				return x64_sdaiErrorQuery();
			}
		}

        //
        //  Geometry Kernel related API Calls
        //

		/// <summary>
		///		owlGetModel                                             (https://rdf.bg/ifcdoc/CS64/owlGetModel.html)
		///
		///	Returns a handle to the model within the Geometry Kernel.
		///
		///	Note: the STEP Engine uses one or more models within the Geometry Kernel to generate design trees
		///		  within the Geometry Kernel. All Geometry Kernel calls can be called with the STEP model handle also,
		///		  however most correct would be to get and use the Geometry Kernel handle.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "owlGetModel")]
		public static extern void x86_owlGetModel(Int32 model, out Int64 owlModel);

		[DllImport(IFCEngineDLL, EntryPoint = "owlGetModel")]
		public static extern void x64_owlGetModel(Int64 model, out Int64 owlModel);

		public static void owlGetModel(Int64 model, out Int64 owlModel)
		{
			if (IntPtr.Size == 4)
			{
				x86_owlGetModel((Int32)model, out Int64 _owlModel);
				owlModel = _owlModel;
			}
			else
			{
				x64_owlGetModel(model, out owlModel);
			}
		}

		/// <summary>
		///		owlConnectModel                                         (https://rdf.bg/ifcdoc/CS64/owlConnectModel.html)
		///
		///	By default a model for the Geometry Modelling Kernel will be created once required on-the-fly.
		///
		///	This call allows a user to use an existing model that will be connected. This connected model
		///	will not be destroyed at closing of the STEP model, i.e. within sdaiCloseModel().
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "owlConnectModel")]
		public static extern byte x86_owlConnectModel(Int32 model, Int64 owlModel);

		[DllImport(IFCEngineDLL, EntryPoint = "owlConnectModel")]
		public static extern byte x64_owlConnectModel(Int64 model, Int64 owlModel);

		public static byte owlConnectModel(Int64 model, Int64 owlModel)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_owlConnectModel((Int32)model, owlModel);
				return _result;
			}
			else
			{
				return x64_owlConnectModel(model, owlModel);
			}
		}

		/// <summary>
		///		owlGetInstance                                          (https://rdf.bg/ifcdoc/CS64/owlGetInstance.html)
		///
		///	Returns a handle to the instance representing the head of design tree within the Geometry Kernel.
		///
		///	Note: the STEP Engine uses one or more models within the Geometry Kernel to generate design trees
		///		  within the Geometry Kernel. All Geometry Kernel calls can be called with the STEP instance handle also,
		///		  however most correct would be to get and use the Geometry Kernel handle.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "owlGetInstance")]
		public static extern void x86_owlGetInstance(Int32 model, Int32 instance, out Int64 owlInstance);

		[DllImport(IFCEngineDLL, EntryPoint = "owlGetInstance")]
		public static extern void x64_owlGetInstance(Int64 model, Int64 instance, out Int64 owlInstance);

		public static void owlGetInstance(Int64 model, Int64 instance, out Int64 owlInstance)
		{
			if (IntPtr.Size == 4)
			{
				x86_owlGetInstance((Int32)model, (Int32)instance, out Int64 _owlInstance);
				owlInstance = _owlInstance;
			}
			else
			{
				x64_owlGetInstance(model, instance, out owlInstance);
			}
		}

		/// <summary>
		///		owlMaterialInstance                                     (https://rdf.bg/ifcdoc/CS64/owlMaterialInstance.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "owlMaterialInstance")]
		public static extern void x86_owlMaterialInstance(Int32 instanceBase, Int32 instanceContext, out Int64 owlInstance);

		[DllImport(IFCEngineDLL, EntryPoint = "owlMaterialInstance")]
		public static extern void x64_owlMaterialInstance(Int64 instanceBase, Int64 instanceContext, out Int64 owlInstance);

		public static void owlMaterialInstance(Int64 instanceBase, Int64 instanceContext, out Int64 owlInstance)
		{
			if (IntPtr.Size == 4)
			{
				x86_owlMaterialInstance((Int32)instanceBase, (Int32)instanceContext, out Int64 _owlInstance);
				owlInstance = _owlInstance;
			}
			else
			{
				x64_owlMaterialInstance(instanceBase, instanceContext, out owlInstance);
			}
		}

		/// <summary>
		///		owlBuildInstance                                        (https://rdf.bg/ifcdoc/CS64/owlBuildInstance.html)
		///
		///	Returns a handle to the instance representing the head of design tree within the Geometry Kernel.
		///	If no design tree is created yet it will be created on-the-fly.
		///
		///	Note: the STEP Engine uses one or more models within the Geometry Kernel to generate design trees
		///		  within the Geometry Kernel. All Geometry Kernel calls can be called with the STEP instance handle also,
		///		  however most correct would be to get and use the Geometry Kernel handle.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "owlBuildInstance")]
		public static extern void x86_owlBuildInstance(Int32 model, Int32 instance, out Int64 owlInstance);

		[DllImport(IFCEngineDLL, EntryPoint = "owlBuildInstance")]
		public static extern void x64_owlBuildInstance(Int64 model, Int64 instance, out Int64 owlInstance);

		public static void owlBuildInstance(Int64 model, Int64 instance, out Int64 owlInstance)
		{
			if (IntPtr.Size == 4)
			{
				x86_owlBuildInstance((Int32)model, (Int32)instance, out Int64 _owlInstance);
				owlInstance = _owlInstance;
			}
			else
			{
				x64_owlBuildInstance(model, instance, out owlInstance);
			}
		}

		/// <summary>
		///		owlBuildInstanceInContext                               (https://rdf.bg/ifcdoc/CS64/owlBuildInstanceInContext.html)
		///
		///	Returns a handle to the instance representing the head of design tree within the Geometry Kernel.
		///	If no design tree is created yet it will be created on-the-fly.
		///
		///	Note: the STEP Engine uses one or more models within the Geometry Kernel to generate design trees
		///		  within the Geometry Kernel. All Geometry Kernel calls can be called with the STEP instance handle also,
		///		  however most correct would be to get and use the Geometry Kernel handle.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "owlBuildInstanceInContext")]
		public static extern void x86_owlBuildInstanceInContext(Int32 instanceBase, Int32 instanceContext, out Int64 owlInstance);

		[DllImport(IFCEngineDLL, EntryPoint = "owlBuildInstanceInContext")]
		public static extern void x64_owlBuildInstanceInContext(Int64 instanceBase, Int64 instanceContext, out Int64 owlInstance);

		public static void owlBuildInstanceInContext(Int64 instanceBase, Int64 instanceContext, out Int64 owlInstance)
		{
			if (IntPtr.Size == 4)
			{
				x86_owlBuildInstanceInContext((Int32)instanceBase, (Int32)instanceContext, out Int64 _owlInstance);
				owlInstance = _owlInstance;
			}
			else
			{
				x64_owlBuildInstanceInContext(instanceBase, instanceContext, out owlInstance);
			}
		}

		/// <summary>
		///		engiInstanceUsesSegmentation                            (https://rdf.bg/ifcdoc/CS64/engiInstanceUsesSegmentation.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiInstanceUsesSegmentation")]
		public static extern byte x86_engiInstanceUsesSegmentation(Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "engiInstanceUsesSegmentation")]
		public static extern byte x64_engiInstanceUsesSegmentation(Int64 instance);

		public static byte engiInstanceUsesSegmentation(Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiInstanceUsesSegmentation((Int32)instance);
				return _result;
			}
			else
			{
				return x64_engiInstanceUsesSegmentation(instance);
			}
		}

		/// <summary>
		///		owlBuildInstances                                       (https://rdf.bg/ifcdoc/CS64/owlBuildInstances.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "owlBuildInstances")]
		public static extern void x86_owlBuildInstances(Int32 model, Int32 instance, out Int64 owlInstanceComplete, out Int64 owlInstanceSolids, out Int64 owlInstanceVoids);

		[DllImport(IFCEngineDLL, EntryPoint = "owlBuildInstances")]
		public static extern void x64_owlBuildInstances(Int64 model, Int64 instance, out Int64 owlInstanceComplete, out Int64 owlInstanceSolids, out Int64 owlInstanceVoids);

		public static void owlBuildInstances(Int64 model, Int64 instance, out Int64 owlInstanceComplete, out Int64 owlInstanceSolids, out Int64 owlInstanceVoids)
		{
			if (IntPtr.Size == 4)
			{
				x86_owlBuildInstances((Int32)model, (Int32)instance, out Int64 _owlInstanceComplete, out Int64 _owlInstanceSolids, out Int64 _owlInstanceVoids);
				owlInstanceComplete = _owlInstanceComplete;
				owlInstanceSolids = _owlInstanceSolids;
				owlInstanceVoids = _owlInstanceVoids;
			}
			else
			{
				x64_owlBuildInstances(model, instance, out owlInstanceComplete, out owlInstanceSolids, out owlInstanceVoids);
			}
		}

		/// <summary>
		///		owlGetMappedItem                                        (https://rdf.bg/ifcdoc/CS64/owlGetMappedItem.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "owlGetMappedItem")]
		public static extern void x86_owlGetMappedItem(Int32 model, Int32 instance, out Int64 owlInstance, out double transformationMatrix);

		[DllImport(IFCEngineDLL, EntryPoint = "owlGetMappedItem")]
		public static extern void x64_owlGetMappedItem(Int64 model, Int64 instance, out Int64 owlInstance, out double transformationMatrix);

		public static void owlGetMappedItem(Int64 model, Int64 instance, out Int64 owlInstance, out double transformationMatrix)
		{
			if (IntPtr.Size == 4)
			{
				x86_owlGetMappedItem((Int32)model, (Int32)instance, out Int64 _owlInstance, out double _transformationMatrix);
				owlInstance = _owlInstance;
				transformationMatrix = _transformationMatrix;
			}
			else
			{
				x64_owlGetMappedItem(model, instance, out owlInstance, out transformationMatrix);
			}
		}

		/// <summary>
		///		getInstanceDerivedPropertiesInModelling                 (https://rdf.bg/ifcdoc/CS64/getInstanceDerivedPropertiesInModelling.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getInstanceDerivedPropertiesInModelling")]
		public static extern Int32 x86_getInstanceDerivedPropertiesInModelling(Int32 model, Int32 instance, out double height, out double width, out double thickness);

		[DllImport(IFCEngineDLL, EntryPoint = "getInstanceDerivedPropertiesInModelling")]
		public static extern Int64 x64_getInstanceDerivedPropertiesInModelling(Int64 model, Int64 instance, out double height, out double width, out double thickness);

		public static Int64 getInstanceDerivedPropertiesInModelling(Int64 model, Int64 instance, out double height, out double width, out double thickness)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getInstanceDerivedPropertiesInModelling((Int32)model, (Int32)instance, out double _height, out double _width, out double _thickness);
				height = _height;
				width = _width;
				thickness = _thickness;
				return _result;
			}
			else
			{
				return x64_getInstanceDerivedPropertiesInModelling(model, instance, out height, out width, out thickness);
			}
		}

		/// <summary>
		///		getInstanceDerivedBoundingBox                           (https://rdf.bg/ifcdoc/CS64/getInstanceDerivedBoundingBox.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getInstanceDerivedBoundingBox")]
		public static extern Int32 x86_getInstanceDerivedBoundingBox(Int32 model, Int32 instance, out double Ox, out double Oy, out double Oz, out double Vx, out double Vy, out double Vz);

		[DllImport(IFCEngineDLL, EntryPoint = "getInstanceDerivedBoundingBox")]
		public static extern Int64 x64_getInstanceDerivedBoundingBox(Int64 model, Int64 instance, out double Ox, out double Oy, out double Oz, out double Vx, out double Vy, out double Vz);

		public static Int64 getInstanceDerivedBoundingBox(Int64 model, Int64 instance, out double Ox, out double Oy, out double Oz, out double Vx, out double Vy, out double Vz)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getInstanceDerivedBoundingBox((Int32)model, (Int32)instance, out double _Ox, out double _Oy, out double _Oz, out double _Vx, out double _Vy, out double _Vz);
				Ox = _Ox;
				Oy = _Oy;
				Oz = _Oz;
				Vx = _Vx;
				Vy = _Vy;
				Vz = _Vz;
				return _result;
			}
			else
			{
				return x64_getInstanceDerivedBoundingBox(model, instance, out Ox, out Oy, out Oz, out Vx, out Vy, out Vz);
			}
		}

		/// <summary>
		///		getInstanceTransformationMatrix                         (https://rdf.bg/ifcdoc/CS64/getInstanceTransformationMatrix.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getInstanceTransformationMatrix")]
		public static extern Int32 x86_getInstanceTransformationMatrix(Int32 model, Int32 instance, out double _11, out double _12, out double _13, out double _14, out double _21, out double _22, out double _23, out double _24, out double _31, out double _32, out double _33, out double _34, out double _41, out double _42, out double _43, out double _44);

		[DllImport(IFCEngineDLL, EntryPoint = "getInstanceTransformationMatrix")]
		public static extern Int64 x64_getInstanceTransformationMatrix(Int64 model, Int64 instance, out double _11, out double _12, out double _13, out double _14, out double _21, out double _22, out double _23, out double _24, out double _31, out double _32, out double _33, out double _34, out double _41, out double _42, out double _43, out double _44);

		public static Int64 getInstanceTransformationMatrix(Int64 model, Int64 instance, out double _11, out double _12, out double _13, out double _14, out double _21, out double _22, out double _23, out double _24, out double _31, out double _32, out double _33, out double _34, out double _41, out double _42, out double _43, out double _44)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getInstanceTransformationMatrix((Int32)model, (Int32)instance, out double __11, out double __12, out double __13, out double __14, out double __21, out double __22, out double __23, out double __24, out double __31, out double __32, out double __33, out double __34, out double __41, out double __42, out double __43, out double __44);
				_11 = __11;
				_12 = __12;
				_13 = __13;
				_14 = __14;
				_21 = __21;
				_22 = __22;
				_23 = __23;
				_24 = __24;
				_31 = __31;
				_32 = __32;
				_33 = __33;
				_34 = __34;
				_41 = __41;
				_42 = __42;
				_43 = __43;
				_44 = __44;
				return _result;
			}
			else
			{
				return x64_getInstanceTransformationMatrix(model, instance, out _11, out _12, out _13, out _14, out _21, out _22, out _23, out _24, out _31, out _32, out _33, out _34, out _41, out _42, out _43, out _44);
			}
		}

		/// <summary>
		///		getInstanceDerivedTransformationMatrix                  (https://rdf.bg/ifcdoc/CS64/getInstanceDerivedTransformationMatrix.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getInstanceDerivedTransformationMatrix")]
		public static extern Int32 x86_getInstanceDerivedTransformationMatrix(Int32 model, Int32 instance, out double _11, out double _12, out double _13, out double _14, out double _21, out double _22, out double _23, out double _24, out double _31, out double _32, out double _33, out double _34, out double _41, out double _42, out double _43, out double _44);

		[DllImport(IFCEngineDLL, EntryPoint = "getInstanceDerivedTransformationMatrix")]
		public static extern Int64 x64_getInstanceDerivedTransformationMatrix(Int64 model, Int64 instance, out double _11, out double _12, out double _13, out double _14, out double _21, out double _22, out double _23, out double _24, out double _31, out double _32, out double _33, out double _34, out double _41, out double _42, out double _43, out double _44);

		public static Int64 getInstanceDerivedTransformationMatrix(Int64 model, Int64 instance, out double _11, out double _12, out double _13, out double _14, out double _21, out double _22, out double _23, out double _24, out double _31, out double _32, out double _33, out double _34, out double _41, out double _42, out double _43, out double _44)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getInstanceDerivedTransformationMatrix((Int32)model, (Int32)instance, out double __11, out double __12, out double __13, out double __14, out double __21, out double __22, out double __23, out double __24, out double __31, out double __32, out double __33, out double __34, out double __41, out double __42, out double __43, out double __44);
				_11 = __11;
				_12 = __12;
				_13 = __13;
				_14 = __14;
				_21 = __21;
				_22 = __22;
				_23 = __23;
				_24 = __24;
				_31 = __31;
				_32 = __32;
				_33 = __33;
				_34 = __34;
				_41 = __41;
				_42 = __42;
				_43 = __43;
				_44 = __44;
				return _result;
			}
			else
			{
				return x64_getInstanceDerivedTransformationMatrix(model, instance, out _11, out _12, out _13, out _14, out _21, out _22, out _23, out _24, out _31, out _32, out _33, out _34, out _41, out _42, out _43, out _44);
			}
		}

		/// <summary>
		///		internalGetBoundingBox                                  (https://rdf.bg/ifcdoc/CS64/internalGetBoundingBox.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "internalGetBoundingBox")]
		public static extern Int32 x86_internalGetBoundingBox(Int32 model, Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "internalGetBoundingBox")]
		public static extern Int64 x64_internalGetBoundingBox(Int64 model, Int64 instance);

		public static Int64 internalGetBoundingBox(Int64 model, Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_internalGetBoundingBox((Int32)model, (Int32)instance);
				return _result;
			}
			else
			{
				return x64_internalGetBoundingBox(model, instance);
			}
		}

		/// <summary>
		///		internalGetCenter                                       (https://rdf.bg/ifcdoc/CS64/internalGetCenter.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "internalGetCenter")]
		public static extern Int32 x86_internalGetCenter(Int32 model, Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "internalGetCenter")]
		public static extern Int64 x64_internalGetCenter(Int64 model, Int64 instance);

		public static Int64 internalGetCenter(Int64 model, Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_internalGetCenter((Int32)model, (Int32)instance);
				return _result;
			}
			else
			{
				return x64_internalGetCenter(model, instance);
			}
		}

		/// <summary>
		///		getRootAxis2Placement                                   (https://rdf.bg/ifcdoc/CS64/getRootAxis2Placement.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getRootAxis2Placement")]
		public static extern Int32 x86_getRootAxis2Placement(Int32 model, bool exclusiveIfHasGeometry);

		[DllImport(IFCEngineDLL, EntryPoint = "getRootAxis2Placement")]
		public static extern Int64 x64_getRootAxis2Placement(Int64 model, bool exclusiveIfHasGeometry);

		public static Int64 getRootAxis2Placement(Int64 model, bool exclusiveIfHasGeometry)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getRootAxis2Placement((Int32)model, exclusiveIfHasGeometry);
				return _result;
			}
			else
			{
				return x64_getRootAxis2Placement(model, exclusiveIfHasGeometry);
			}
		}

		/// <summary>
		///		getGlobalPlacement                                      (https://rdf.bg/ifcdoc/CS64/getGlobalPlacement.html)
		///
		///	The call getGlobalPlacement is meant to be used together with setGlobalPlacement(..) and allows you to get and adjust the placement of a model.
		///	This is all done semantically, i.e. it can be seen as a derived call representing a small SDAI function adjust (in case of set) the
		///	origin of a model. 
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getGlobalPlacement")]
		public static extern Int32 x86_getGlobalPlacement(Int32 model, out double origin);

		[DllImport(IFCEngineDLL, EntryPoint = "getGlobalPlacement")]
		public static extern Int64 x64_getGlobalPlacement(Int64 model, out double origin);

		public static Int64 getGlobalPlacement(Int64 model, out double origin)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getGlobalPlacement((Int32)model, out double _origin);
				origin = _origin;
				return _result;
			}
			else
			{
				return x64_getGlobalPlacement(model, out origin);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "getGlobalPlacement")]
		public static extern Int32 x86_getGlobalPlacement(Int32 model, double[] origin);

		[DllImport(IFCEngineDLL, EntryPoint = "getGlobalPlacement")]
		public static extern Int64 x64_getGlobalPlacement(Int64 model, double[] origin);

		public static Int64 getGlobalPlacement(Int64 model, double[] origin)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getGlobalPlacement((Int32)model, origin);
				return _result;
			}
			else
			{
				return x64_getGlobalPlacement(model, origin);
			}
		}

		/// <summary>
		///		setGlobalPlacement                                      (https://rdf.bg/ifcdoc/CS64/setGlobalPlacement.html)
		///
		///	The call setGlobalPlacement allows you to adjust the placement of a model.
		///	This is all done semantically, i.e. it can be seen as a derived call representing a small SDAI function adjust the origin of a model. 
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "setGlobalPlacement")]
		public static extern Int32 x86_setGlobalPlacement(Int32 model, ref double origin, bool includeRotation);

		[DllImport(IFCEngineDLL, EntryPoint = "setGlobalPlacement")]
		public static extern Int64 x64_setGlobalPlacement(Int64 model, ref double origin, bool includeRotation);

		public static Int64 setGlobalPlacement(Int64 model, ref double origin, bool includeRotation)
		{
			if (IntPtr.Size == 4)
			{
				double _origin = (double)origin;
				var _result = x86_setGlobalPlacement((Int32)model, ref _origin, includeRotation);
				origin = _origin;
				return _result;
			}
			else
			{
				return x64_setGlobalPlacement(model, ref origin, includeRotation);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "setGlobalPlacement")]
		public static extern Int32 x86_setGlobalPlacement(Int32 model, double[] origin, bool includeRotation);

		[DllImport(IFCEngineDLL, EntryPoint = "setGlobalPlacement")]
		public static extern Int64 x64_setGlobalPlacement(Int64 model, double[] origin, bool includeRotation);

		public static Int64 setGlobalPlacement(Int64 model, double[] origin, bool includeRotation)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_setGlobalPlacement((Int32)model, origin, includeRotation);
				return _result;
			}
			else
			{
				return x64_setGlobalPlacement(model, origin, includeRotation);
			}
		}

		/// <summary>
		///		getTimeStamp                                            (https://rdf.bg/ifcdoc/CS64/getTimeStamp.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getTimeStamp")]
		public static extern Int32 x86_getTimeStamp(Int32 model);

		[DllImport(IFCEngineDLL, EntryPoint = "getTimeStamp")]
		public static extern Int64 x64_getTimeStamp(Int64 model);

		public static Int64 getTimeStamp(Int64 model)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getTimeStamp((Int32)model);
				return _result;
			}
			else
			{
				return x64_getTimeStamp(model);
			}
		}

		/// <summary>
		///		setInstanceReference                                    (https://rdf.bg/ifcdoc/CS64/setInstanceReference.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "setInstanceReference")]
		public static extern Int32 x86_setInstanceReference(Int32 instance, Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "setInstanceReference")]
		public static extern Int64 x64_setInstanceReference(Int64 instance, Int64 value);

		public static Int64 setInstanceReference(Int64 instance, Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_setInstanceReference((Int32)instance, (Int32)value);
				return _result;
			}
			else
			{
				return x64_setInstanceReference(instance, value);
			}
		}

		/// <summary>
		///		getInstanceReference                                    (https://rdf.bg/ifcdoc/CS64/getInstanceReference.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getInstanceReference")]
		public static extern Int32 x86_getInstanceReference(Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "getInstanceReference")]
		public static extern Int64 x64_getInstanceReference(Int64 instance);

		public static Int64 getInstanceReference(Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getInstanceReference((Int32)instance);
				return _result;
			}
			else
			{
				return x64_getInstanceReference(instance);
			}
		}

		/// <summary>
		///		inferenceInstance                                       (https://rdf.bg/ifcdoc/CS64/inferenceInstance.html)
		///
		///	This call allows certain constructs to complete implicitly already available data.
		///	Specifically for IFC4.3 and higher calls using the instances of the following entities are supported:
		///		IfcAlignment	   => in case business logic is defined and not geometrically representation is available yet
		///							  the geometrical representation will be constructed on the fly, i.e.
		///							  an IfcCompositeCurve with IfcCurveSegment instances for the horizontal alignment 
		///							  an IfcGradientCurve with IfcCurveSegment instances for the vertical alignment 
		///							  an IfcSegmentedReferenceCurve with IfcCurveSegment instances for the cant alignment
		///		IfcLinearPlacement => in case CartesianPosition is empty the internally calculated matrix will be
		///							  represented as an IfcAxis2Placement
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "inferenceInstance")]
		public static extern Int32 x86_inferenceInstance(Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "inferenceInstance")]
		public static extern Int64 x64_inferenceInstance(Int64 instance);

		public static Int64 inferenceInstance(Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_inferenceInstance((Int32)instance);
				return _result;
			}
			else
			{
				return x64_inferenceInstance(instance);
			}
		}

		/// <summary>
		///		sdaiValidateSchemaInstance                              (https://rdf.bg/ifcdoc/CS64/sdaiValidateSchemaInstance.html)
		///
		///	...
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiValidateSchemaInstance")]
		public static extern Int32 x86_sdaiValidateSchemaInstance(Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiValidateSchemaInstance")]
		public static extern Int64 x64_sdaiValidateSchemaInstance(Int64 instance);

		public static Int64 sdaiValidateSchemaInstance(Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiValidateSchemaInstance((Int32)instance);
				return _result;
			}
			else
			{
				return x64_sdaiValidateSchemaInstance(instance);
			}
		}

        //
        //  Deprecated API Calls (GENERIC)
        //

		/// <summary>
		///		engiGetEntityAttributeIndex                             (https://rdf.bg/ifcdoc/CS64/engiGetEntityAttributeIndex.html)
		///
		///	This call is deprecated, please use call engiGetAttrIndexBN(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityAttributeIndex")]
		public static extern Int32 x86_engiGetEntityAttributeIndex(Int32 entity, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityAttributeIndex")]
		public static extern Int64 x64_engiGetEntityAttributeIndex(Int64 entity, string attributeName);

		public static Int64 engiGetEntityAttributeIndex(Int64 entity, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityAttributeIndex((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiGetEntityAttributeIndex(entity, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityAttributeIndex")]
		public static extern Int32 x86_engiGetEntityAttributeIndex(Int32 entity, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityAttributeIndex")]
		public static extern Int64 x64_engiGetEntityAttributeIndex(Int64 entity, byte[] attributeName);

		public static Int64 engiGetEntityAttributeIndex(Int64 entity, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityAttributeIndex((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiGetEntityAttributeIndex(entity, attributeName);
			}
		}

		/// <summary>
		///		engiGetEntityAttributeIndexEx                           (https://rdf.bg/ifcdoc/CS64/engiGetEntityAttributeIndexEx.html)
		///
		///	This call is deprecated, please use call engiGetAttrIndexExBN(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityAttributeIndexEx")]
		public static extern Int32 x86_engiGetEntityAttributeIndexEx(Int32 entity, string attributeName, bool countedWithParents, bool countedWithInverse);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityAttributeIndexEx")]
		public static extern Int64 x64_engiGetEntityAttributeIndexEx(Int64 entity, string attributeName, bool countedWithParents, bool countedWithInverse);

		public static Int64 engiGetEntityAttributeIndexEx(Int64 entity, string attributeName, bool countedWithParents, bool countedWithInverse)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityAttributeIndexEx((Int32)entity, attributeName, countedWithParents, countedWithInverse);
				return _result;
			}
			else
			{
				return x64_engiGetEntityAttributeIndexEx(entity, attributeName, countedWithParents, countedWithInverse);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityAttributeIndexEx")]
		public static extern Int32 x86_engiGetEntityAttributeIndexEx(Int32 entity, byte[] attributeName, bool countedWithParents, bool countedWithInverse);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityAttributeIndexEx")]
		public static extern Int64 x64_engiGetEntityAttributeIndexEx(Int64 entity, byte[] attributeName, bool countedWithParents, bool countedWithInverse);

		public static Int64 engiGetEntityAttributeIndexEx(Int64 entity, byte[] attributeName, bool countedWithParents, bool countedWithInverse)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityAttributeIndexEx((Int32)entity, attributeName, countedWithParents, countedWithInverse);
				return _result;
			}
			else
			{
				return x64_engiGetEntityAttributeIndexEx(entity, attributeName, countedWithParents, countedWithInverse);
			}
		}

		/// <summary>
		///		engiGetEntityArgumentName                               (https://rdf.bg/ifcdoc/CS64/engiGetEntityArgumentName.html)
		///
		///	This call is deprecated, please use call engiGetAttrNameByIndex(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityArgumentName")]
		public static extern IntPtr x86_engiGetEntityArgumentName(Int32 entity, Int32 index, Int32 valueType, out IntPtr attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityArgumentName")]
		public static extern IntPtr x64_engiGetEntityArgumentName(Int64 entity, Int64 index, Int64 valueType, out IntPtr attributeName);

		public static IntPtr engiGetEntityArgumentName(Int64 entity, Int64 index, Int64 valueType, out IntPtr attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityArgumentName((Int32)entity, (Int32)index, (Int32)valueType, out IntPtr _attributeName);
				attributeName = _attributeName;
				return _result;
			}
			else
			{
				return x64_engiGetEntityArgumentName(entity, index, valueType, out attributeName);
			}
		}

		/// <summary>
		///		engiGetEntityArgumentType                               (https://rdf.bg/ifcdoc/CS64/engiGetEntityArgumentType.html)
		///
		///	This call is deprecated, please use call engiGetAttrTypeByIndex(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityArgumentType")]
		public static extern void x86_engiGetEntityArgumentType(Int32 entity, Int32 index, out Int32 attributeType);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityArgumentType")]
		public static extern void x64_engiGetEntityArgumentType(Int64 entity, Int64 index, out Int64 attributeType);

		public static void engiGetEntityArgumentType(Int64 entity, Int64 index, out Int64 attributeType)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiGetEntityArgumentType((Int32)entity, (Int32)index, out Int32 _attributeType);
				attributeType = _attributeType;
			}
			else
			{
				x64_engiGetEntityArgumentType(entity, index, out attributeType);
			}
		}

		/// <summary>
		///		engiGetAttrOptional                                     (https://rdf.bg/ifcdoc/CS64/engiGetAttrOptional.html)
		///
		///	This call is deprecated, please use call engiIsAttrOptional(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrOptional")]
		public static extern Int32 x86_engiGetAttrOptional(Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrOptional")]
		public static extern Int64 x64_engiGetAttrOptional(Int64 attribute);

		public static Int64 engiGetAttrOptional(Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrOptional((Int32)attribute);
				return _result;
			}
			else
			{
				return x64_engiGetAttrOptional(attribute);
			}
		}

		/// <summary>
		///		engiGetAttrOptionalBN                                   (https://rdf.bg/ifcdoc/CS64/engiGetAttrOptionalBN.html)
		///
		///	This call is deprecated, please use call engiIsAttrOptionalBN(..) instead.
		///
		///	Technically engiGetAttrOptionalBN will transform into the following call
		///		engiGetAttrOptional(
		///				sdaiGetAttrDefinition(
		///						entity,
		///						attributeName
		///					)
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrOptionalBN")]
		public static extern Int32 x86_engiGetAttrOptionalBN(Int32 entity, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrOptionalBN")]
		public static extern Int64 x64_engiGetAttrOptionalBN(Int64 entity, string attributeName);

		public static Int64 engiGetAttrOptionalBN(Int64 entity, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrOptionalBN((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiGetAttrOptionalBN(entity, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrOptionalBN")]
		public static extern Int32 x86_engiGetAttrOptionalBN(Int32 entity, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrOptionalBN")]
		public static extern Int64 x64_engiGetAttrOptionalBN(Int64 entity, byte[] attributeName);

		public static Int64 engiGetAttrOptionalBN(Int64 entity, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrOptionalBN((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiGetAttrOptionalBN(entity, attributeName);
			}
		}

		/// <summary>
		///		engiGetAttrInverse                                      (https://rdf.bg/ifcdoc/CS64/engiGetAttrInverse.html)
		///
		///	This call is deprecated, please use call engiIsAttrInverse(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrInverse")]
		public static extern Int32 x86_engiGetAttrInverse(Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrInverse")]
		public static extern Int64 x64_engiGetAttrInverse(Int64 attribute);

		public static Int64 engiGetAttrInverse(Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrInverse((Int32)attribute);
				return _result;
			}
			else
			{
				return x64_engiGetAttrInverse(attribute);
			}
		}

		/// <summary>
		///		engiGetAttrInverseBN                                    (https://rdf.bg/ifcdoc/CS64/engiGetAttrInverseBN.html)
		///
		///	This call is deprecated, please use call engiIsAttrInverseBN(..) instead.
		///
		///	Technically engiGetAttrInverseBN will transform into the following call
		///		engiGetAttrInverse(
		///				sdaiGetAttrDefinition(
		///						entity,
		///						attributeName
		///					)
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrInverseBN")]
		public static extern Int32 x86_engiGetAttrInverseBN(Int32 entity, string attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrInverseBN")]
		public static extern Int64 x64_engiGetAttrInverseBN(Int64 entity, string attributeName);

		public static Int64 engiGetAttrInverseBN(Int64 entity, string attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrInverseBN((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiGetAttrInverseBN(entity, attributeName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrInverseBN")]
		public static extern Int32 x86_engiGetAttrInverseBN(Int32 entity, byte[] attributeName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrInverseBN")]
		public static extern Int64 x64_engiGetAttrInverseBN(Int64 entity, byte[] attributeName);

		public static Int64 engiGetAttrInverseBN(Int64 entity, byte[] attributeName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrInverseBN((Int32)entity, attributeName);
				return _result;
			}
			else
			{
				return x64_engiGetAttrInverseBN(entity, attributeName);
			}
		}

		/// <summary>
		///		engiAttrIsInverse                                       (https://rdf.bg/ifcdoc/CS64/engiAttrIsInverse.html)
		///
		///	This call is deprecated, please use call engiIsAttrInverse(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiAttrIsInverse")]
		public static extern Int32 x86_engiAttrIsInverse(Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "engiAttrIsInverse")]
		public static extern Int64 x64_engiAttrIsInverse(Int64 attribute);

		public static Int64 engiAttrIsInverse(Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiAttrIsInverse((Int32)attribute);
				return _result;
			}
			else
			{
				return x64_engiAttrIsInverse(attribute);
			}
		}

		/// <summary>
		///		engiGetAttrDomain                                       (https://rdf.bg/ifcdoc/CS64/engiGetAttrDomain.html)
		///
		///	This call is deprecated, please use call engiGetAttrDomainName(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDomain")]
		public static extern IntPtr x86_engiGetAttrDomain(Int32 attribute, out IntPtr domainName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDomain")]
		public static extern IntPtr x64_engiGetAttrDomain(Int64 attribute, out IntPtr domainName);

		public static IntPtr engiGetAttrDomain(Int64 attribute, out IntPtr domainName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrDomain((Int32)attribute, out IntPtr _domainName);
				domainName = _domainName;
				return _result;
			}
			else
			{
				return x64_engiGetAttrDomain(attribute, out domainName);
			}
		}

		/// <summary>
		///		engiGetAttrDomainBN                                     (https://rdf.bg/ifcdoc/CS64/engiGetAttrDomainBN.html)
		///
		///	This call is deprecated, please use call engiGetAttrDomainNameBN(..) instead.
		///
		///	Technically engiGetAttrDomainBN will transform into the following call
		///		engiGetAttrDomain(
		///				sdaiGetAttrDefinition(
		///						entity,
		///						attributeName
		///					),
		///				domainName
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDomainBN")]
		public static extern IntPtr x86_engiGetAttrDomainBN(Int32 entity, string attributeName, out IntPtr domainName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDomainBN")]
		public static extern IntPtr x64_engiGetAttrDomainBN(Int64 entity, string attributeName, out IntPtr domainName);

		public static IntPtr engiGetAttrDomainBN(Int64 entity, string attributeName, out IntPtr domainName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrDomainBN((Int32)entity, attributeName, out IntPtr _domainName);
				domainName = _domainName;
				return _result;
			}
			else
			{
				return x64_engiGetAttrDomainBN(entity, attributeName, out domainName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDomainBN")]
		public static extern IntPtr x86_engiGetAttrDomainBN(Int32 entity, byte[] attributeName, out IntPtr domainName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttrDomainBN")]
		public static extern IntPtr x64_engiGetAttrDomainBN(Int64 entity, byte[] attributeName, out IntPtr domainName);

		public static IntPtr engiGetAttrDomainBN(Int64 entity, byte[] attributeName, out IntPtr domainName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttrDomainBN((Int32)entity, attributeName, out IntPtr _domainName);
				domainName = _domainName;
				return _result;
			}
			else
			{
				return x64_engiGetAttrDomainBN(entity, attributeName, out domainName);
			}
		}

		/// <summary>
		///		engiGetEntityIsAbstract                                 (https://rdf.bg/ifcdoc/CS64/engiGetEntityIsAbstract.html)
		///
		///	This call is deprecated, please use call engiIsEntityAbstract(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityIsAbstract")]
		public static extern Int32 x86_engiGetEntityIsAbstract(Int32 entity);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityIsAbstract")]
		public static extern Int64 x64_engiGetEntityIsAbstract(Int64 entity);

		public static Int64 engiGetEntityIsAbstract(Int64 entity)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityIsAbstract((Int32)entity);
				return _result;
			}
			else
			{
				return x64_engiGetEntityIsAbstract(entity);
			}
		}

		/// <summary>
		///		engiGetEntityIsAbstractBN                               (https://rdf.bg/ifcdoc/CS64/engiGetEntityIsAbstractBN.html)
		///
		///	This call is deprecated, please use call engiIsEntityAbstractBN(..) instead.
		///
		///	Technically engiGetEntityIsAbstractBN will transform into the following call
		///		engiGetEntityIsAbstract(
		///				sdaiGetEntity(
		///						model,
		///						entityName
		///					)
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityIsAbstractBN")]
		public static extern Int32 x86_engiGetEntityIsAbstractBN(Int32 model, string entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityIsAbstractBN")]
		public static extern Int64 x64_engiGetEntityIsAbstractBN(Int64 model, string entityName);

		public static Int64 engiGetEntityIsAbstractBN(Int64 model, string entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityIsAbstractBN((Int32)model, entityName);
				return _result;
			}
			else
			{
				return x64_engiGetEntityIsAbstractBN(model, entityName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityIsAbstractBN")]
		public static extern Int32 x86_engiGetEntityIsAbstractBN(Int32 model, byte[] entityName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityIsAbstractBN")]
		public static extern Int64 x64_engiGetEntityIsAbstractBN(Int64 model, byte[] entityName);

		public static Int64 engiGetEntityIsAbstractBN(Int64 model, byte[] entityName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityIsAbstractBN((Int32)model, entityName);
				return _result;
			}
			else
			{
				return x64_engiGetEntityIsAbstractBN(model, entityName);
			}
		}

		/// <summary>
		///		engiGetAttributeTraits                                  (https://rdf.bg/ifcdoc/CS64/engiGetAttributeTraits.html)
		///
		///	This call is deprecated, please use call engiGetAttrTraits(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttributeTraits")]
		public static extern void x86_engiGetAttributeTraits(Int32 attribute, out IntPtr name, Int32 definingEntity, out bool isExplicit, out bool isInverse, out enum_express_attr_type attrType, Int32 domainEntity, out Int32 aggregationDefinition, out bool isOptional);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttributeTraits")]
		public static extern void x64_engiGetAttributeTraits(Int64 attribute, out IntPtr name, Int64 definingEntity, out bool isExplicit, out bool isInverse, out enum_express_attr_type attrType, Int64 domainEntity, out Int64 aggregationDefinition, out bool isOptional);

		public static void engiGetAttributeTraits(Int64 attribute, out IntPtr name, Int64 definingEntity, out bool isExplicit, out bool isInverse, out enum_express_attr_type attrType, Int64 domainEntity, out Int64 aggregationDefinition, out bool isOptional)
		{
			if (IntPtr.Size == 4)
			{
				x86_engiGetAttributeTraits((Int32)attribute, out IntPtr _name, (Int32)definingEntity, out bool _isExplicit, out bool _isInverse, out enum_express_attr_type _attrType, (Int32)domainEntity, out Int32 _aggregationDefinition, out bool _isOptional);
				name = _name;
				isExplicit = _isExplicit;
				isInverse = _isInverse;
				attrType = _attrType;
				aggregationDefinition = _aggregationDefinition;
				isOptional = _isOptional;
			}
			else
			{
				x64_engiGetAttributeTraits(attribute, out name, definingEntity, out isExplicit, out isInverse, out attrType, domainEntity, out aggregationDefinition, out isOptional);
			}
		}

		/// <summary>
		///		engiGetEntityNoArguments                                (https://rdf.bg/ifcdoc/CS64/engiGetEntityNoArguments.html)
		///
		///	This call is deprecated, please use call engiGetEntityNoAttributes(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityNoArguments")]
		public static extern Int32 x86_engiGetEntityNoArguments(Int32 entity);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityNoArguments")]
		public static extern Int64 x64_engiGetEntityNoArguments(Int64 entity);

		public static Int64 engiGetEntityNoArguments(Int64 entity)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityNoArguments((Int32)entity);
				return _result;
			}
			else
			{
				return x64_engiGetEntityNoArguments(entity);
			}
		}

		/// <summary>
		///		engiGetArgumentType                                     (https://rdf.bg/ifcdoc/CS64/engiGetArgumentType.html)
		///
		///	This call is deprecated, please use call engiGetAttrType(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetArgumentType")]
		public static extern Int32 x86_engiGetArgumentType(Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetArgumentType")]
		public static extern Int64 x64_engiGetArgumentType(Int64 attribute);

		public static Int64 engiGetArgumentType(Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetArgumentType((Int32)attribute);
				return _result;
			}
			else
			{
				return x64_engiGetArgumentType(attribute);
			}
		}

		/// <summary>
		///		engiGetAttributeType                                    (https://rdf.bg/ifcdoc/CS64/engiGetAttributeType.html)
		///
		///	This call is deprecated, please use call engiGetAttrType(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttributeType")]
		public static extern Int32 x86_engiGetAttributeType(Int32 attribute);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAttributeType")]
		public static extern Int64 x64_engiGetAttributeType(Int64 attribute);

		public static Int64 engiGetAttributeType(Int64 attribute)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAttributeType((Int32)attribute);
				return _result;
			}
			else
			{
				return x64_engiGetAttributeType(attribute);
			}
		}

		/// <summary>
		///		engiGetEntityArgumentIndex                              (https://rdf.bg/ifcdoc/CS64/engiGetEntityArgumentIndex.html)
		///
		///	This call is deprecated, please use call engiGetAttrIndexBN(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityArgumentIndex")]
		public static extern Int32 x86_engiGetEntityArgumentIndex(Int32 entity, string argumentName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityArgumentIndex")]
		public static extern Int64 x64_engiGetEntityArgumentIndex(Int64 entity, string argumentName);

		public static Int64 engiGetEntityArgumentIndex(Int64 entity, string argumentName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityArgumentIndex((Int32)entity, argumentName);
				return _result;
			}
			else
			{
				return x64_engiGetEntityArgumentIndex(entity, argumentName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityArgumentIndex")]
		public static extern Int32 x86_engiGetEntityArgumentIndex(Int32 entity, byte[] argumentName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityArgumentIndex")]
		public static extern Int64 x64_engiGetEntityArgumentIndex(Int64 entity, byte[] argumentName);

		public static Int64 engiGetEntityArgumentIndex(Int64 entity, byte[] argumentName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityArgumentIndex((Int32)entity, argumentName);
				return _result;
			}
			else
			{
				return x64_engiGetEntityArgumentIndex(entity, argumentName);
			}
		}

		/// <summary>
		///		engiGetAggrElement                                      (https://rdf.bg/ifcdoc/CS64/engiGetAggrElement.html)
		///
		///	This call is deprecated, please use call sdaiGetAggrByIndex(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrElement")]
		public static extern Int32 x86_engiGetAggrElement(Int32 aggregate, Int32 index, Int32 valueType, out bool value);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrElement")]
		public static extern Int64 x64_engiGetAggrElement(Int64 aggregate, Int64 index, Int64 valueType, out bool value);

		public static Int64 engiGetAggrElement(Int64 aggregate, Int64 index, Int64 valueType, out bool value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAggrElement((Int32)aggregate, (Int32)index, (Int32)valueType, out bool _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_engiGetAggrElement(aggregate, index, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrElement")]
		public static extern Int32 x86_engiGetAggrElement(Int32 aggregate, Int32 index, Int32 valueType, out Int32 value);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrElement")]
		public static extern Int64 x64_engiGetAggrElement(Int64 aggregate, Int64 index, Int64 valueType, out Int64 value);

		public static Int64 engiGetAggrElement(Int64 aggregate, Int64 index, Int64 valueType, out Int64 value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAggrElement((Int32)aggregate, (Int32)index, (Int32)valueType, out Int32 _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_engiGetAggrElement(aggregate, index, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrElement")]
		public static extern Int32 x86_engiGetAggrElement(Int32 aggregate, Int32 index, Int32 valueType, out double value);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrElement")]
		public static extern Int64 x64_engiGetAggrElement(Int64 aggregate, Int64 index, Int64 valueType, out double value);

		public static Int64 engiGetAggrElement(Int64 aggregate, Int64 index, Int64 valueType, out double value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAggrElement((Int32)aggregate, (Int32)index, (Int32)valueType, out double _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_engiGetAggrElement(aggregate, index, valueType, out value);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrElement")]
		public static extern Int32 x86_engiGetAggrElement(Int32 aggregate, Int32 index, Int32 valueType, out IntPtr value);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetAggrElement")]
		public static extern Int64 x64_engiGetAggrElement(Int64 aggregate, Int64 index, Int64 valueType, out IntPtr value);

		public static Int64 engiGetAggrElement(Int64 aggregate, Int64 index, Int64 valueType, out IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetAggrElement((Int32)aggregate, (Int32)index, (Int32)valueType, out IntPtr _value);
				value = _value;
				return _result;
			}
			else
			{
				return x64_engiGetAggrElement(aggregate, index, valueType, out value);
			}
		}

		/// <summary>
		///		engiGetEntityArgument                                   (https://rdf.bg/ifcdoc/CS64/engiGetEntityArgument.html)
		///
		///	This call is deprecated, please use call sdaiGetAttrDefinition(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityArgument")]
		public static extern Int32 x86_engiGetEntityArgument(Int32 entity, string argumentName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityArgument")]
		public static extern Int64 x64_engiGetEntityArgument(Int64 entity, string argumentName);

		public static Int64 engiGetEntityArgument(Int64 entity, string argumentName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityArgument((Int32)entity, argumentName);
				return _result;
			}
			else
			{
				return x64_engiGetEntityArgument(entity, argumentName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityArgument")]
		public static extern Int32 x86_engiGetEntityArgument(Int32 entity, byte[] argumentName);

		[DllImport(IFCEngineDLL, EntryPoint = "engiGetEntityArgument")]
		public static extern Int64 x64_engiGetEntityArgument(Int64 entity, byte[] argumentName);

		public static Int64 engiGetEntityArgument(Int64 entity, byte[] argumentName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_engiGetEntityArgument((Int32)entity, argumentName);
				return _result;
			}
			else
			{
				return x64_engiGetEntityArgument(entity, argumentName);
			}
		}

		/// <summary>
		///		sdaiGetADBTypePathx                                     (https://rdf.bg/ifcdoc/CS64/sdaiGetADBTypePathx.html)
		///
		///	This call is deprecated, please use call sdaiGetADBTypePath(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetADBTypePathx")]
		public static extern IntPtr x86_sdaiGetADBTypePathx(Int32 ADB, Int32 typeNameNumber, out IntPtr path);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiGetADBTypePathx")]
		public static extern IntPtr x64_sdaiGetADBTypePathx(Int64 ADB, Int64 typeNameNumber, out IntPtr path);

		public static IntPtr sdaiGetADBTypePathx(Int64 ADB, Int64 typeNameNumber, out IntPtr path)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiGetADBTypePathx((Int32)ADB, (Int32)typeNameNumber, out IntPtr _path);
				path = _path;
				return _result;
			}
			else
			{
				return x64_sdaiGetADBTypePathx(ADB, typeNameNumber, out path);
			}
		}

		/// <summary>
		///		xxxxOpenModelByStream                                   (https://rdf.bg/ifcdoc/CS64/xxxxOpenModelByStream.html)
		///
		///	This call is deprecated, please use call engiOpenModelByStream(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "xxxxOpenModelByStream")]
		public static extern Int32 x86_xxxxOpenModelByStream(Int32 repository, [MarshalAs(UnmanagedType.FunctionPtr)] WriteCallBackFunction callback, string schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "xxxxOpenModelByStream")]
		public static extern Int64 x64_xxxxOpenModelByStream(Int64 repository, [MarshalAs(UnmanagedType.FunctionPtr)] WriteCallBackFunction callback, string schemaName);

		public static Int64 xxxxOpenModelByStream(Int64 repository, [MarshalAs(UnmanagedType.FunctionPtr)] WriteCallBackFunction callback, string schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_xxxxOpenModelByStream((Int32)repository, callback, schemaName);
				return _result;
			}
			else
			{
				return x64_xxxxOpenModelByStream(repository, callback, schemaName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "xxxxOpenModelByStream")]
		public static extern Int32 x86_xxxxOpenModelByStream(Int32 repository, [MarshalAs(UnmanagedType.FunctionPtr)] WriteCallBackFunction callback, byte[] schemaName);

		[DllImport(IFCEngineDLL, EntryPoint = "xxxxOpenModelByStream")]
		public static extern Int64 x64_xxxxOpenModelByStream(Int64 repository, [MarshalAs(UnmanagedType.FunctionPtr)] WriteCallBackFunction callback, byte[] schemaName);

		public static Int64 xxxxOpenModelByStream(Int64 repository, [MarshalAs(UnmanagedType.FunctionPtr)] WriteCallBackFunction callback, byte[] schemaName)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_xxxxOpenModelByStream((Int32)repository, callback, schemaName);
				return _result;
			}
			else
			{
				return x64_xxxxOpenModelByStream(repository, callback, schemaName);
			}
		}

		/// <summary>
		///		sdaiplusGetAggregationType                              (https://rdf.bg/ifcdoc/CS64/sdaiplusGetAggregationType.html)
		///
		///	This call is deprecated, please use call .... instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "sdaiplusGetAggregationType")]
		public static extern Int32 x86_sdaiplusGetAggregationType(Int32 instance, Int32 aggregate);

		[DllImport(IFCEngineDLL, EntryPoint = "sdaiplusGetAggregationType")]
		public static extern Int64 x64_sdaiplusGetAggregationType(Int64 instance, Int64 aggregate);

		public static Int64 sdaiplusGetAggregationType(Int64 instance, Int64 aggregate)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_sdaiplusGetAggregationType((Int32)instance, (Int32)aggregate);
				return _result;
			}
			else
			{
				return x64_sdaiplusGetAggregationType(instance, aggregate);
			}
		}

		/// <summary>
		///		xxxxGetAttrType                                         (https://rdf.bg/ifcdoc/CS64/xxxxGetAttrType.html)
		///
		///	This call is deprecated, please use calls engiGetAttrType(..) instead.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetAttrType")]
		public static extern Int32 x86_xxxxGetAttrType(Int32 instance, Int32 attribute, out IntPtr attributeType);

		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetAttrType")]
		public static extern Int64 x64_xxxxGetAttrType(Int64 instance, Int64 attribute, out IntPtr attributeType);

		public static Int64 xxxxGetAttrType(Int64 instance, Int64 attribute, out IntPtr attributeType)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_xxxxGetAttrType((Int32)instance, (Int32)attribute, out IntPtr _attributeType);
				attributeType = _attributeType;
				return _result;
			}
			else
			{
				return x64_xxxxGetAttrType(instance, attribute, out attributeType);
			}
		}

		/// <summary>
		///		xxxxGetAttrTypeBN                                       (https://rdf.bg/ifcdoc/CS64/xxxxGetAttrTypeBN.html)
		///
		///	This call is deprecated, please use calls engiGetAttrTypeBN(..) instead.
		///
		///	Technically it will transform into the following call
		///		xxxxGetAttrType(
		///				instance,
		///				sdaiGetAttrDefinition(
		///						sdaiGetInstanceType(
		///								instance
		///							),
		///						attributeName
		///					),
		///				attributeType
		///			);
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetAttrTypeBN")]
		public static extern Int32 x86_xxxxGetAttrTypeBN(Int32 instance, string attributeName, out IntPtr attributeType);

		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetAttrTypeBN")]
		public static extern Int64 x64_xxxxGetAttrTypeBN(Int64 instance, string attributeName, out IntPtr attributeType);

		public static Int64 xxxxGetAttrTypeBN(Int64 instance, string attributeName, out IntPtr attributeType)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_xxxxGetAttrTypeBN((Int32)instance, attributeName, out IntPtr _attributeType);
				attributeType = _attributeType;
				return _result;
			}
			else
			{
				return x64_xxxxGetAttrTypeBN(instance, attributeName, out attributeType);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetAttrTypeBN")]
		public static extern Int32 x86_xxxxGetAttrTypeBN(Int32 instance, byte[] attributeName, out IntPtr attributeType);

		[DllImport(IFCEngineDLL, EntryPoint = "xxxxGetAttrTypeBN")]
		public static extern Int64 x64_xxxxGetAttrTypeBN(Int64 instance, byte[] attributeName, out IntPtr attributeType);

		public static Int64 xxxxGetAttrTypeBN(Int64 instance, byte[] attributeName, out IntPtr attributeType)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_xxxxGetAttrTypeBN((Int32)instance, attributeName, out IntPtr _attributeType);
				attributeType = _attributeType;
				return _result;
			}
			else
			{
				return x64_xxxxGetAttrTypeBN(instance, attributeName, out attributeType);
			}
		}

		/// <summary>
		///		GetSPFFHeaderItemUnicode                                (https://rdf.bg/ifcdoc/CS64/GetSPFFHeaderItemUnicode.html)
		///
		///	This call is deprecated, please use call GetSPFFHeaderItem instead
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "GetSPFFHeaderItemUnicode")]
		public static extern Int32 x86_GetSPFFHeaderItemUnicode(Int32 model, Int32 itemIndex, Int32 itemSubIndex, byte[] buffer, Int32 bufferLength);

		[DllImport(IFCEngineDLL, EntryPoint = "GetSPFFHeaderItemUnicode")]
		public static extern Int64 x64_GetSPFFHeaderItemUnicode(Int64 model, Int64 itemIndex, Int64 itemSubIndex, byte[] buffer, Int64 bufferLength);

		public static Int64 GetSPFFHeaderItemUnicode(Int64 model, Int64 itemIndex, Int64 itemSubIndex, byte[] buffer, Int64 bufferLength)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_GetSPFFHeaderItemUnicode((Int32)model, (Int32)itemIndex, (Int32)itemSubIndex, buffer, (Int32)bufferLength);
				return _result;
			}
			else
			{
				return x64_GetSPFFHeaderItemUnicode(model, itemIndex, itemSubIndex, buffer, bufferLength);
			}
		}

        //
        //  Validation
        //

		/// <summary>
		///		validateSetOptions                                      (https://rdf.bg/ifcdoc/CS64/validateSetOptions.html)
		///
		///	Allows to set a time limit in seconds, setting to 0 means no time limit.
		///	Allows to set a count limit, setting to 0 means no count limit.
		///	Allows to hide redundant issues.
		///
		///		bit 0:	(__KNOWN_ENTITY)					entity is defined in the schema
		///		bit 1:	(__NO_OF_ARGUMENTS)					number of arguments
		///		bit 2:	(__ARGUMENT_EXPRESS_TYPE)			argument value is correct entity, defined type or enumeration value
		///		bit 3:	(__ARGUMENT_PRIM_TYPE)				argument value has correct primitive type
		///		bit 4:	(__REQUIRED_ARGUMENTS)				non-optional arguments values are provided
		///		bit 5:	(__ARRGEGATION_EXPECTED)			aggregation is provided when expected
		///		bit 6:	(__AGGREGATION_NOT_EXPECTED)		aggregation is not used when not expected
		///		bit 7:	(__AGGREGATION_SIZE)				aggregation size
		///		bit 8:	(__AGGREGATION_UNIQUE)				elements in aggregations are unique when required
		///		bit 9:	(__COMPLEX_INSTANCE)				complex instances contains full parent chains
		///		bit 10:	(__REFERENCE_EXISTS)				referenced instance exists
		///		bit 11:	(__ABSTRACT_ENTITY)					abstract entity should not instantiate
		///		bit 12:	(__WHERE_RULE)						where-rule check
		///		bit 13:	(__UNIQUE_RULE)						unique-rule check
		///		bit 14:	(__STAR_USAGE)						* is used only for derived arguments
		///		bit 15:	(__CALL_ARGUMENT)					validateModel/validateInstance function argument should be model/instance
		///		bit 63:	(__INTERNAL_ERROR)					unspecified error
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "validateSetOptions")]
		public static extern void x86_validateSetOptions(Int32 timeLimitSeconds, Int32 issueCntLimit, bool showEachIssueOnce, UInt64 issueTypes, UInt64 mask);

		[DllImport(IFCEngineDLL, EntryPoint = "validateSetOptions")]
		public static extern void x64_validateSetOptions(Int64 timeLimitSeconds, Int64 issueCntLimit, bool showEachIssueOnce, UInt64 issueTypes, UInt64 mask);

		public static void validateSetOptions(Int64 timeLimitSeconds, Int64 issueCntLimit, bool showEachIssueOnce, UInt64 issueTypes, UInt64 mask)
		{
			if (IntPtr.Size == 4)
			{
				x86_validateSetOptions((Int32)timeLimitSeconds, (Int32)issueCntLimit, showEachIssueOnce, issueTypes, mask);
			}
			else
			{
				x64_validateSetOptions(timeLimitSeconds, issueCntLimit, showEachIssueOnce, issueTypes, mask);
			}
		}

		/// <summary>
		///		validateGetOptions                                      (https://rdf.bg/ifcdoc/CS64/validateGetOptions.html)
		///
		///	Allows to get the time limit in seconds, value 0 means no time limit, input can be left to NULL if not relevant.
		///	Allows to get the count limit, value 0 means no count limit, input can be left to NULL if not relevant.
		///	Allows to get hide redundant issues, input can be left to NULL if not relevant.
		///	Return value is the issueTypes enabled according to the mask given.
		///
		///		bit 0:	(__KNOWN_ENTITY)					entity is defined in the schema
		///		bit 1:	(__NO_OF_ARGUMENTS)					number of arguments
		///		bit 2:	(__ARGUMENT_EXPRESS_TYPE)			argument value is correct entity, defined type or enumeration value
		///		bit 3:	(__ARGUMENT_PRIM_TYPE)				argument value has correct primitive type
		///		bit 4:	(__REQUIRED_ARGUMENTS)				non-optional arguments values are provided
		///		bit 5:	(__ARRGEGATION_EXPECTED)			aggregation is provided when expected
		///		bit 6:	(__AGGREGATION_NOT_EXPECTED)		aggregation is not used when not expected
		///		bit 7:	(__AGGREGATION_SIZE)				aggregation size
		///		bit 8:	(__AGGREGATION_UNIQUE)				elements in aggregations are unique when required
		///		bit 9:	(__COMPLEX_INSTANCE)				complex instances contains full parent chains
		///		bit 10:	(__REFERENCE_EXISTS)				referenced instance exists
		///		bit 11:	(__ABSTRACT_ENTITY)					abstract entity should not instantiate
		///		bit 12:	(__WHERE_RULE)						where-rule check
		///		bit 13:	(__UNIQUE_RULE)						unique-rule check
		///		bit 14:	(__STAR_USAGE)						* is used only for derived arguments
		///		bit 15:	(__CALL_ARGUMENT)					validateModel/validateInstance function argument should be model/instance
		///		bit 63:	(__INTERNAL_ERROR)					unspecified error
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "validateGetOptions")]
		public static extern UInt64 x86_validateGetOptions(out Int32 timeLimitSeconds, out Int32 issueCntLimit, out bool showEachIssueOnce, UInt64 mask);

		[DllImport(IFCEngineDLL, EntryPoint = "validateGetOptions")]
		public static extern UInt64 x64_validateGetOptions(out Int64 timeLimitSeconds, out Int64 issueCntLimit, out bool showEachIssueOnce, UInt64 mask);

		public static UInt64 validateGetOptions(out Int64 timeLimitSeconds, out Int64 issueCntLimit, out bool showEachIssueOnce, UInt64 mask)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_validateGetOptions(out Int32 _timeLimitSeconds, out Int32 _issueCntLimit, out bool _showEachIssueOnce, mask);
				timeLimitSeconds = _timeLimitSeconds;
				issueCntLimit = _issueCntLimit;
				showEachIssueOnce = _showEachIssueOnce;
				return _result;
			}
			else
			{
				return x64_validateGetOptions(out timeLimitSeconds, out issueCntLimit, out showEachIssueOnce, mask);
			}
		}

		/// <summary>
		///		validateModel                                           (https://rdf.bg/ifcdoc/CS64/validateModel.html)
		///
		///	Apply validation of a model
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "validateModel")]
		public static extern Int32 x86_validateModel(Int32 model);

		[DllImport(IFCEngineDLL, EntryPoint = "validateModel")]
		public static extern Int64 x64_validateModel(Int64 model);

		public static Int64 validateModel(Int64 model)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_validateModel((Int32)model);
				return _result;
			}
			else
			{
				return x64_validateModel(model);
			}
		}

		/// <summary>
		///		validateInstance                                        (https://rdf.bg/ifcdoc/CS64/validateInstance.html)
		///
		///	Apply validation of an instance
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "validateInstance")]
		public static extern Int32 x86_validateInstance(Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "validateInstance")]
		public static extern Int64 x64_validateInstance(Int64 instance);

		public static Int64 validateInstance(Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_validateInstance((Int32)instance);
				return _result;
			}
			else
			{
				return x64_validateInstance(instance);
			}
		}

		/// <summary>
		///		validateFreeResults                                     (https://rdf.bg/ifcdoc/CS64/validateFreeResults.html)
		///
		///	Clean validation results
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "validateFreeResults")]
		public static extern void x86_validateFreeResults(Int32 results);

		[DllImport(IFCEngineDLL, EntryPoint = "validateFreeResults")]
		public static extern void x64_validateFreeResults(Int64 results);

		public static void validateFreeResults(Int64 results)
		{
			if (IntPtr.Size == 4)
			{
				x86_validateFreeResults((Int32)results);
			}
			else
			{
				x64_validateFreeResults(results);
			}
		}

		/// <summary>
		///		validateGetFirstIssue                                   (https://rdf.bg/ifcdoc/CS64/validateGetFirstIssue.html)
		///
		///	Get first issue from validation results.
		///	If no issues inside validation results or validation results is NULL it will return NULL.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "validateGetFirstIssue")]
		public static extern Int32 x86_validateGetFirstIssue(Int32 results);

		[DllImport(IFCEngineDLL, EntryPoint = "validateGetFirstIssue")]
		public static extern Int64 x64_validateGetFirstIssue(Int64 results);

		public static Int64 validateGetFirstIssue(Int64 results)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_validateGetFirstIssue((Int32)results);
				return _result;
			}
			else
			{
				return x64_validateGetFirstIssue(results);
			}
		}

		/// <summary>
		///		validateGetNextIssue                                    (https://rdf.bg/ifcdoc/CS64/validateGetNextIssue.html)
		///
		///	Get next issue based on a given issue.
		///	If no issues left or validation issue is NULL it will return NULL.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "validateGetNextIssue")]
		public static extern Int32 x86_validateGetNextIssue(Int32 issue);

		[DllImport(IFCEngineDLL, EntryPoint = "validateGetNextIssue")]
		public static extern Int64 x64_validateGetNextIssue(Int64 issue);

		public static Int64 validateGetNextIssue(Int64 issue)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_validateGetNextIssue((Int32)issue);
				return _result;
			}
			else
			{
				return x64_validateGetNextIssue(issue);
			}
		}

		/// <summary>
		///		validateGetStatus                                       (https://rdf.bg/ifcdoc/CS64/validateGetStatus.html)
		///
		///	Return value is the issueStatus (enum_validation_status):
		///
		///		value 0:	(__NONE)						no status set
		///		value 1:	(__COMPLETE_ALL)				all issues proceed
		///		value 2:	(__COMPLETE_NOT_ALL)			completed but some issues were excluded by option settings
		///		value 3:	(__TIME_EXCEED)					validation was finished because of reach time limit
		///		value 4:	(__COUNT_EXCEED)				validation was finished because of reach of issue's numbers limit
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "validateGetStatus")]
		public static extern enum_validation_status x86_validateGetStatus(Int32 results);

		[DllImport(IFCEngineDLL, EntryPoint = "validateGetStatus")]
		public static extern enum_validation_status x64_validateGetStatus(Int64 results);

		public static enum_validation_status validateGetStatus(Int64 results)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_validateGetStatus((Int32)results);
				return _result;
			}
			else
			{
				return x64_validateGetStatus(results);
			}
		}

		/// <summary>
		///		validateGetIssueType                                    (https://rdf.bg/ifcdoc/CS64/validateGetIssueType.html)
		///
		///	Return value is the issueType (enum_validation_type):
		///
		///		bit 0:	(__KNOWN_ENTITY)					entity is defined in the schema
		///		bit 1:	(__NO_OF_ARGUMENTS)					number of arguments
		///		bit 2:	(__ARGUMENT_EXPRESS_TYPE)			argument value is correct entity, defined type or enumeration value
		///		bit 3:	(__ARGUMENT_PRIM_TYPE)				argument value has correct primitive type
		///		bit 4:	(__REQUIRED_ARGUMENTS)				non-optional arguments values are provided
		///		bit 5:	(__ARRGEGATION_EXPECTED)			aggregation is provided when expected
		///		bit 6:	(__AGGREGATION_NOT_EXPECTED)		aggregation is not used when not expected
		///		bit 7:	(__AGGREGATION_SIZE)				aggregation size
		///		bit 8:	(__AGGREGATION_UNIQUE)				elements in aggregations are unique when required
		///		bit 9:	(__COMPLEX_INSTANCE)				complex instances contains full parent chains
		///		bit 10:	(__REFERENCE_EXISTS)				referenced instance exists
		///		bit 11:	(__ABSTRACT_ENTITY)					abstract entity should not instantiate
		///		bit 12:	(__WHERE_RULE)						where-rule check
		///		bit 13:	(__UNIQUE_RULE)						unique-rule check
		///		bit 14:	(__STAR_USAGE)						* is used only for derived arguments
		///		bit 15:	(__CALL_ARGUMENT)					validateModel/validateInstance function argument should be model/instance
		///		bit 63:	(__INTERNAL_ERROR)					unspecified error
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "validateGetIssueType")]
		public static extern UInt64 x86_validateGetIssueType(Int32 issue);

		[DllImport(IFCEngineDLL, EntryPoint = "validateGetIssueType")]
		public static extern UInt64 x64_validateGetIssueType(Int64 issue);

		public static UInt64 validateGetIssueType(Int64 issue)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_validateGetIssueType((Int32)issue);
				return _result;
			}
			else
			{
				return x64_validateGetIssueType(issue);
			}
		}

		/// <summary>
		///		validateGetInstance                                     (https://rdf.bg/ifcdoc/CS64/validateGetInstance.html)
		///
		///	Returns the (first) instance related to the given issue.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "validateGetInstance")]
		public static extern Int32 x86_validateGetInstance(Int32 issue);

		[DllImport(IFCEngineDLL, EntryPoint = "validateGetInstance")]
		public static extern Int64 x64_validateGetInstance(Int64 issue);

		public static Int64 validateGetInstance(Int64 issue)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_validateGetInstance((Int32)issue);
				return _result;
			}
			else
			{
				return x64_validateGetInstance(issue);
			}
		}

		/// <summary>
		///		validateGetInstanceRelated                              (https://rdf.bg/ifcdoc/CS64/validateGetInstanceRelated.html)
		///
		///	Returns the second instance related to the given issue (if relevant).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "validateGetInstanceRelated")]
		public static extern Int32 x86_validateGetInstanceRelated(Int32 issue);

		[DllImport(IFCEngineDLL, EntryPoint = "validateGetInstanceRelated")]
		public static extern Int64 x64_validateGetInstanceRelated(Int64 issue);

		public static Int64 validateGetInstanceRelated(Int64 issue)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_validateGetInstanceRelated((Int32)issue);
				return _result;
			}
			else
			{
				return x64_validateGetInstanceRelated(issue);
			}
		}

		/// <summary>
		///		validateGetEntity                                       (https://rdf.bg/ifcdoc/CS64/validateGetEntity.html)
		///
		///	Returns the entity handle related to the given issue (if relevant).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "validateGetEntity")]
		public static extern Int32 x86_validateGetEntity(Int32 issue);

		[DllImport(IFCEngineDLL, EntryPoint = "validateGetEntity")]
		public static extern Int64 x64_validateGetEntity(Int64 issue);

		public static Int64 validateGetEntity(Int64 issue)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_validateGetEntity((Int32)issue);
				return _result;
			}
			else
			{
				return x64_validateGetEntity(issue);
			}
		}

		/// <summary>
		///		validateGetAttr                                         (https://rdf.bg/ifcdoc/CS64/validateGetAttr.html)
		///
		///	Returns the attribute handle related to the given issue (if relevant).
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "validateGetAttr")]
		public static extern Int32 x86_validateGetAttr(Int32 issue);

		[DllImport(IFCEngineDLL, EntryPoint = "validateGetAttr")]
		public static extern Int64 x64_validateGetAttr(Int64 issue);

		public static Int64 validateGetAttr(Int64 issue)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_validateGetAttr((Int32)issue);
				return _result;
			}
			else
			{
				return x64_validateGetAttr(issue);
			}
		}

		/// <summary>
		///		validateGetAggrLevel                                    (https://rdf.bg/ifcdoc/CS64/validateGetAggrLevel.html)
		///
		///	Specifies nesting level of aggregation or 0.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "validateGetAggrLevel")]
		public static extern ValidationIssueLevel x86_validateGetAggrLevel(Int32 issue);

		[DllImport(IFCEngineDLL, EntryPoint = "validateGetAggrLevel")]
		public static extern ValidationIssueLevel x64_validateGetAggrLevel(Int64 issue);

		public static ValidationIssueLevel validateGetAggrLevel(Int64 issue)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_validateGetAggrLevel((Int32)issue);
				return _result;
			}
			else
			{
				return x64_validateGetAggrLevel(issue);
			}
		}

		/// <summary>
		///		validateGetAggrIndArray                                 (https://rdf.bg/ifcdoc/CS64/validateGetAggrIndArray.html)
		///
		///	Array of indices for each aggregation size is aggrLevel.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "validateGetAggrIndArray")]
		public static extern Int32 x86_validateGetAggrIndArray(Int32 issue);

		[DllImport(IFCEngineDLL, EntryPoint = "validateGetAggrIndArray")]
		public static extern Int64 x64_validateGetAggrIndArray(Int64 issue);

		public static Int64 validateGetAggrIndArray(Int64 issue)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_validateGetAggrIndArray((Int32)issue);
				return _result;
			}
			else
			{
				return x64_validateGetAggrIndArray(issue);
			}
		}

		/// <summary>
		///		validateGetIssueLevel                                   (https://rdf.bg/ifcdoc/CS64/validateGetIssueLevel.html)
		///
		///	Returns the issue level (i.e. severity of the issue) of the issue given as input.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "validateGetIssueLevel")]
		public static extern Int32 x86_validateGetIssueLevel(Int32 issue);

		[DllImport(IFCEngineDLL, EntryPoint = "validateGetIssueLevel")]
		public static extern Int64 x64_validateGetIssueLevel(Int64 issue);

		public static Int64 validateGetIssueLevel(Int64 issue)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_validateGetIssueLevel((Int32)issue);
				return _result;
			}
			else
			{
				return x64_validateGetIssueLevel(issue);
			}
		}

		/// <summary>
		///		validateGetDescription                                  (https://rdf.bg/ifcdoc/CS64/validateGetDescription.html)
		///
		///	Returns the description text of the issue given as input.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "validateGetDescription")]
		public static extern IntPtr x86_validateGetDescription(Int32 issue);

		[DllImport(IFCEngineDLL, EntryPoint = "validateGetDescription")]
		public static extern IntPtr x64_validateGetDescription(Int64 issue);

		public static IntPtr validateGetDescription(Int64 issue)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_validateGetDescription((Int32)issue);
				return _result;
			}
			else
			{
				return x64_validateGetDescription(issue);
			}
		}

        //
        //  Deprecated API Calls (GEOMETRY)
        //

		/// <summary>
		///		initializeModellingInstance                             (https://rdf.bg/ifcdoc/CS64/initializeModellingInstance.html)
		///
		///	This call is deprecated, please use call CalculateInstance().
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "initializeModellingInstance")]
		public static extern Int32 x86_initializeModellingInstance(Int32 model, out Int32 noVertices, out Int32 noIndices, double scale, Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "initializeModellingInstance")]
		public static extern Int64 x64_initializeModellingInstance(Int64 model, out Int64 noVertices, out Int64 noIndices, double scale, Int64 instance);

		public static Int64 initializeModellingInstance(Int64 model, out Int64 noVertices, out Int64 noIndices, double scale, Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_initializeModellingInstance((Int32)model, out Int32 _noVertices, out Int32 _noIndices, scale, (Int32)instance);
				noVertices = _noVertices;
				noIndices = _noIndices;
				return _result;
			}
			else
			{
				return x64_initializeModellingInstance(model, out noVertices, out noIndices, scale, instance);
			}
		}

		/// <summary>
		///		finalizeModelling                                       (https://rdf.bg/ifcdoc/CS64/finalizeModelling.html)
		///
		///	This call is deprecated, please use call UpdateInstanceVertexBuffer() and UpdateInstanceIndexBuffer().
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "finalizeModelling")]
		public static extern Int32 x86_finalizeModelling(Int32 model, out float vertices, out Int32 indices, Int32 FVF);

		[DllImport(IFCEngineDLL, EntryPoint = "finalizeModelling")]
		public static extern Int64 x64_finalizeModelling(Int64 model, out float vertices, out Int64 indices, Int64 FVF);

		public static Int64 finalizeModelling(Int64 model, out float vertices, out Int64 indices, Int64 FVF)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_finalizeModelling((Int32)model, out float _vertices, out Int32 _indices, (Int32)FVF);
				vertices = _vertices;
				indices = _indices;
				return _result;
			}
			else
			{
				return x64_finalizeModelling(model, out vertices, out indices, FVF);
			}
		}

		/// <summary>
		///		getInstanceInModelling                                  (https://rdf.bg/ifcdoc/CS64/getInstanceInModelling.html)
		///
		///	This call is deprecated, there is no direct/easy replacement although the functionality is present. If you still use this call please contact RDF to find a solution together.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getInstanceInModelling")]
		public static extern Int32 x86_getInstanceInModelling(Int32 model, Int32 instance, Int32 mode, out Int32 startVertex, out Int32 startIndex, out Int32 primitiveCount);

		[DllImport(IFCEngineDLL, EntryPoint = "getInstanceInModelling")]
		public static extern Int64 x64_getInstanceInModelling(Int64 model, Int64 instance, Int64 mode, out Int64 startVertex, out Int64 startIndex, out Int64 primitiveCount);

		public static Int64 getInstanceInModelling(Int64 model, Int64 instance, Int64 mode, out Int64 startVertex, out Int64 startIndex, out Int64 primitiveCount)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getInstanceInModelling((Int32)model, (Int32)instance, (Int32)mode, out Int32 _startVertex, out Int32 _startIndex, out Int32 _primitiveCount);
				startVertex = _startVertex;
				startIndex = _startIndex;
				primitiveCount = _primitiveCount;
				return _result;
			}
			else
			{
				return x64_getInstanceInModelling(model, instance, mode, out startVertex, out startIndex, out primitiveCount);
			}
		}

		/// <summary>
		///		setVertexOffset                                         (https://rdf.bg/ifcdoc/CS64/setVertexOffset.html)
		///
		///	This call is deprecated, please use call SetVertexBufferOffset().
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "setVertexOffset")]
		public static extern void x86_setVertexOffset(Int32 model, double x, double y, double z);

		[DllImport(IFCEngineDLL, EntryPoint = "setVertexOffset")]
		public static extern void x64_setVertexOffset(Int64 model, double x, double y, double z);

		public static void setVertexOffset(Int64 model, double x, double y, double z)
		{
			if (IntPtr.Size == 4)
			{
				x86_setVertexOffset((Int32)model, x, y, z);
			}
			else
			{
				x64_setVertexOffset(model, x, y, z);
			}
		}

		/// <summary>
		///		setFormat                                               (https://rdf.bg/ifcdoc/CS64/setFormat.html)
		///
		///	This call is deprecated, please use call SetFormat().
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "setFormat")]
		public static extern void x86_setFormat(Int32 model, Int32 setting, Int32 mask);

		[DllImport(IFCEngineDLL, EntryPoint = "setFormat")]
		public static extern void x64_setFormat(Int64 model, Int64 setting, Int64 mask);

		public static void setFormat(Int64 model, Int64 setting, Int64 mask)
		{
			if (IntPtr.Size == 4)
			{
				x86_setFormat((Int32)model, (Int32)setting, (Int32)mask);
			}
			else
			{
				x64_setFormat(model, setting, mask);
			}
		}

		/// <summary>
		///		getConceptualFaceCnt                                    (https://rdf.bg/ifcdoc/CS64/getConceptualFaceCnt.html)
		///
		///	This call is deprecated, please use call GetConceptualFaceCnt().
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getConceptualFaceCnt")]
		public static extern Int32 x86_getConceptualFaceCnt(Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "getConceptualFaceCnt")]
		public static extern Int64 x64_getConceptualFaceCnt(Int64 instance);

		public static Int64 getConceptualFaceCnt(Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getConceptualFaceCnt((Int32)instance);
				return _result;
			}
			else
			{
				return x64_getConceptualFaceCnt(instance);
			}
		}

		/// <summary>
		///		getConceptualFaceEx                                     (https://rdf.bg/ifcdoc/CS64/getConceptualFaceEx.html)
		///
		///	This call is deprecated, please use call GetConceptualFaceEx().
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "getConceptualFaceEx")]
		public static extern Int32 x86_getConceptualFaceEx(Int32 instance, Int32 index, out Int32 startIndexTriangles, out Int32 noIndicesTriangles, out Int32 startIndexLines, out Int32 noIndicesLines, out Int32 startIndexPoints, out Int32 noIndicesPoints, out Int32 startIndexFacePolygons, out Int32 noIndicesFacePolygons, out Int32 startIndexConceptualFacePolygons, out Int32 noIndicesConceptualFacePolygons);

		[DllImport(IFCEngineDLL, EntryPoint = "getConceptualFaceEx")]
		public static extern Int64 x64_getConceptualFaceEx(Int64 instance, Int64 index, out Int64 startIndexTriangles, out Int64 noIndicesTriangles, out Int64 startIndexLines, out Int64 noIndicesLines, out Int64 startIndexPoints, out Int64 noIndicesPoints, out Int64 startIndexFacePolygons, out Int64 noIndicesFacePolygons, out Int64 startIndexConceptualFacePolygons, out Int64 noIndicesConceptualFacePolygons);

		public static Int64 getConceptualFaceEx(Int64 instance, Int64 index, out Int64 startIndexTriangles, out Int64 noIndicesTriangles, out Int64 startIndexLines, out Int64 noIndicesLines, out Int64 startIndexPoints, out Int64 noIndicesPoints, out Int64 startIndexFacePolygons, out Int64 noIndicesFacePolygons, out Int64 startIndexConceptualFacePolygons, out Int64 noIndicesConceptualFacePolygons)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_getConceptualFaceEx((Int32)instance, (Int32)index, out Int32 _startIndexTriangles, out Int32 _noIndicesTriangles, out Int32 _startIndexLines, out Int32 _noIndicesLines, out Int32 _startIndexPoints, out Int32 _noIndicesPoints, out Int32 _startIndexFacePolygons, out Int32 _noIndicesFacePolygons, out Int32 _startIndexConceptualFacePolygons, out Int32 _noIndicesConceptualFacePolygons);
				startIndexTriangles = _startIndexTriangles;
				noIndicesTriangles = _noIndicesTriangles;
				startIndexLines = _startIndexLines;
				noIndicesLines = _noIndicesLines;
				startIndexPoints = _startIndexPoints;
				noIndicesPoints = _noIndicesPoints;
				startIndexFacePolygons = _startIndexFacePolygons;
				noIndicesFacePolygons = _noIndicesFacePolygons;
				startIndexConceptualFacePolygons = _startIndexConceptualFacePolygons;
				noIndicesConceptualFacePolygons = _noIndicesConceptualFacePolygons;
				return _result;
			}
			else
			{
				return x64_getConceptualFaceEx(instance, index, out startIndexTriangles, out noIndicesTriangles, out startIndexLines, out noIndicesLines, out startIndexPoints, out noIndicesPoints, out startIndexFacePolygons, out noIndicesFacePolygons, out startIndexConceptualFacePolygons, out noIndicesConceptualFacePolygons);
			}
		}

		/// <summary>
		///		createGeometryConversion                                (https://rdf.bg/ifcdoc/CS64/createGeometryConversion.html)
		///
		///	This call is deprecated, please use call owlBuildInstance.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "createGeometryConversion")]
		public static extern void x86_createGeometryConversion(Int32 instance, out Int64 owlInstance);

		[DllImport(IFCEngineDLL, EntryPoint = "createGeometryConversion")]
		public static extern void x64_createGeometryConversion(Int64 instance, out Int64 owlInstance);

		public static void createGeometryConversion(Int64 instance, out Int64 owlInstance)
		{
			if (IntPtr.Size == 4)
			{
				x86_createGeometryConversion((Int32)instance, out Int64 _owlInstance);
				owlInstance = _owlInstance;
			}
			else
			{
				x64_createGeometryConversion(instance, out owlInstance);
			}
		}

		/// <summary>
		///		convertInstance                                         (https://rdf.bg/ifcdoc/CS64/convertInstance.html)
		///
		///	This call is deprecated, please use call owlBuildInstance.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "convertInstance")]
		public static extern void x86_convertInstance(Int32 instance);

		[DllImport(IFCEngineDLL, EntryPoint = "convertInstance")]
		public static extern void x64_convertInstance(Int64 instance);

		public static void convertInstance(Int64 instance)
		{
			if (IntPtr.Size == 4)
			{
				x86_convertInstance((Int32)instance);
			}
			else
			{
				x64_convertInstance(instance);
			}
		}

		/// <summary>
		///		initializeModellingInstanceEx                           (https://rdf.bg/ifcdoc/CS64/initializeModellingInstanceEx.html)
		///
		///	This call is deprecated, please use call CalculateInstance().
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "initializeModellingInstanceEx")]
		public static extern Int32 x86_initializeModellingInstanceEx(Int32 model, out Int32 noVertices, out Int32 noIndices, double scale, Int32 instance, Int32 instanceList);

		[DllImport(IFCEngineDLL, EntryPoint = "initializeModellingInstanceEx")]
		public static extern Int64 x64_initializeModellingInstanceEx(Int64 model, out Int64 noVertices, out Int64 noIndices, double scale, Int64 instance, Int64 instanceList);

		public static Int64 initializeModellingInstanceEx(Int64 model, out Int64 noVertices, out Int64 noIndices, double scale, Int64 instance, Int64 instanceList)
		{
			if (IntPtr.Size == 4)
			{
				var _result = x86_initializeModellingInstanceEx((Int32)model, out Int32 _noVertices, out Int32 _noIndices, scale, (Int32)instance, (Int32)instanceList);
				noVertices = _noVertices;
				noIndices = _noIndices;
				return _result;
			}
			else
			{
				return x64_initializeModellingInstanceEx(model, out noVertices, out noIndices, scale, instance, instanceList);
			}
		}

		/// <summary>
		///		exportModellingAsOWL                                    (https://rdf.bg/ifcdoc/CS64/exportModellingAsOWL.html)
		///
		///	This call is deprecated, please contact us if you use this call.
		/// </summary>
		[DllImport(IFCEngineDLL, EntryPoint = "exportModellingAsOWL")]
		public static extern void x86_exportModellingAsOWL(Int32 model, string fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "exportModellingAsOWL")]
		public static extern void x64_exportModellingAsOWL(Int64 model, string fileName);

		public static void exportModellingAsOWL(Int64 model, string fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_exportModellingAsOWL((Int32)model, fileName);
			}
			else
			{
				x64_exportModellingAsOWL(model, fileName);
			}
		}

		[DllImport(IFCEngineDLL, EntryPoint = "exportModellingAsOWL")]
		public static extern void x86_exportModellingAsOWL(Int32 model, byte[] fileName);

		[DllImport(IFCEngineDLL, EntryPoint = "exportModellingAsOWL")]
		public static extern void x64_exportModellingAsOWL(Int64 model, byte[] fileName);

		public static void exportModellingAsOWL(Int64 model, byte[] fileName)
		{
			if (IntPtr.Size == 4)
			{
				x86_exportModellingAsOWL((Int32)model, fileName);
			}
			else
			{
				x64_exportModellingAsOWL(model, fileName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private static long getStringType(long valueType)
		{
			switch (valueType)
			{
				case sdaiSTRING:
				case sdaiUNICODE:
					return sdaiUNICODE;

				case sdaiEXPRESSSTRING:
				case sdaiENUM:
				case sdaiLOGICAL:
				case sdaiBINARY:
					return valueType;
			}
			return 0;
		}

		/// <summary>
		/// 
		/// </summary>
		private static string marshalPtrToString(long valueType, IntPtr ptr)
		{
		    switch (valueType)
		    {
				case sdaiUNICODE:
					return Marshal.PtrToStringUni(ptr);

				case sdaiEXPRESSSTRING:
					return Marshal.PtrToStringAnsi(ptr);

				case sdaiENUM:
				case sdaiLOGICAL:
				case sdaiBINARY:
					{
						var unicode = getStringUnicode();
						if (unicode == 0)
							return Marshal.PtrToStringAnsi(ptr);
						else if (unicode == 1 || unicode == 2)
							return Marshal.PtrToStringUni(ptr);
					}
					break;
		    }
		    return null;
		}
	}
}
