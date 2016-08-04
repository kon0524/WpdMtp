using PortableDeviceApiLib;
using System;

namespace WpdMtpLib
{
    public static class WpdProperty
    {
        /**************************************/
        /*      ROPERTY COMMON COMMAND        */
        /**************************************/
        public static _tagpropertykey WPD_PROPERTY_COMMON_COMMAND_CATEGORY = new _tagpropertykey()
        {
            fmtid = new Guid(0xF0422A9C, 0x5DC8, 0x4440, 0xB5, 0xBD, 0x5D, 0xF2, 0x88, 0x35, 0x65, 0x8A),
            pid = 1001
        };

        public static _tagpropertykey WPD_PROPERTY_COMMON_COMMAND_ID = new _tagpropertykey()
        {
            fmtid = new Guid(0xF0422A9C, 0x5DC8, 0x4440, 0xB5, 0xBD, 0x5D, 0xF2, 0x88, 0x35, 0x65, 0x8A),
            pid = 1002
        };

        /**************************************/
        /*          COMMAND_MTP_EXT           */
        /**************************************/
        public static _tagpropertykey WPD_COMMAND_MTP_EXT_EXECUTE_COMMAND_WITHOUT_DATA_PHASE = new _tagpropertykey()
        {
            fmtid = new Guid(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56),
            pid = 12
        };

        public static _tagpropertykey WPD_COMMAND_MTP_EXT_EXECUTE_COMMAND_WITH_DATA_TO_READ = new _tagpropertykey()
        {
            fmtid = new Guid(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56),
            pid = 13
        };

        public static _tagpropertykey WPD_COMMAND_MTP_EXT_EXECUTE_COMMAND_WITH_DATA_TO_WRITE = new _tagpropertykey()
        {
            fmtid = new Guid(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56),
            pid = 14
        };

        public static _tagpropertykey WPD_COMMAND_MTP_EXT_READ_DATA = new _tagpropertykey()
        {
            fmtid = new Guid(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56),
            pid = 15
        };

        public static _tagpropertykey WPD_COMMAND_MTP_EXT_WRITE_DATA = new _tagpropertykey()
        {
            fmtid = new Guid(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56),
            pid = 16
        };

        public static _tagpropertykey WPD_COMMAND_MTP_EXT_END_DATA_TRANSFER = new _tagpropertykey()
        {
            fmtid = new Guid(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56),
            pid = 17
        };

        /**************************************/
        /*         PROPERTY_MTP_EXT           */
        /**************************************/
        public static _tagpropertykey WPD_PROPERTY_MTP_EXT_OPERATION_CODE = new _tagpropertykey()
        {
            fmtid = new Guid(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56),
            pid = 1001
        };

        public static _tagpropertykey WPD_PROPERTY_MTP_EXT_OPERATION_PARAMS = new _tagpropertykey()
        {
            fmtid = new Guid(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56),
            pid = 1002
        };

        public static _tagpropertykey WPD_PROPERTY_MTP_EXT_RESPONSE_CODE = new _tagpropertykey()
        {
            fmtid = new Guid(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56),
            pid = 1003
        };

        public static _tagpropertykey WPD_PROPERTY_MTP_EXT_RESPONSE_PARAMS = new _tagpropertykey()
        {
            fmtid = new Guid(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56),
            pid = 1004
        };

        public static _tagpropertykey WPD_PROPERTY_MTP_EXT_TRANSFER_CONTEXT = new _tagpropertykey()
        {
            fmtid = new Guid(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56),
            pid = 1006
        };

        public static _tagpropertykey WPD_PROPERTY_MTP_EXT_TRANSFER_TOTAL_DATA_SIZE = new _tagpropertykey()
        {
            fmtid = new Guid(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56),
            pid = 1007
        };

        public static _tagpropertykey WPD_PROPERTY_MTP_EXT_TRANSFER_NUM_BYTES_TO_READ = new _tagpropertykey()
        {
            fmtid = new Guid(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56),
            pid = 1008
        };

        public static _tagpropertykey WPD_PROPERTY_MTP_EXT_TRANSFER_NUM_BYTES_TO_WRITE = new _tagpropertykey()
        {
            fmtid = new Guid(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56),
            pid = 1010
        };

        public static _tagpropertykey WPD_PROPERTY_MTP_EXT_TRANSFER_NUM_BYTES_WRITTEN = new _tagpropertykey()
        {
            fmtid = new Guid(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56),
            pid = 1011
        };

        public static _tagpropertykey WPD_PROPERTY_MTP_EXT_TRANSFER_DATA = new _tagpropertykey()
        {
            fmtid = new Guid(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56),
            pid = 1012
        };

        /**************************************/
        /*          EVENT_PARAMETER           */
        /**************************************/
        public static _tagpropertykey WPD_EVENT_PARAMETER_EVENT_ID = new _tagpropertykey()
        {
            fmtid = new Guid(0x15AB1953, 0xF817, 0x4FEF, 0xA9, 0x21, 0x56, 0x76, 0xE8, 0x38, 0xF6, 0xE0),
            pid = 3
        };
        public static _tagpropertykey WPD_EVENT_DEVICE_CAPABILITIES_UPDATED = new _tagpropertykey()
        {
            fmtid = new Guid(0x15AB1953, 0xF817, 0x4FEF, 0xA9, 0x21, 0x56, 0x76, 0xE8, 0x38, 0xF6, 0xE0),
            pid = 3
        };

        /**************************************/
        /*               OTHER                */
        /**************************************/
        public static _tagpropertykey WPD_DEVICE_PROTOCOL = new _tagpropertykey()
        {
            fmtid = new Guid(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC),
            pid = 6
        };
        public static _tagpropertykey WPD_DEVICE_TYPE = new _tagpropertykey()
        {
            fmtid = new Guid(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC),
            pid = 15
        };

        public static _tagpropertykey WPD_OBJECT_ID = new _tagpropertykey()
        {
            fmtid = new Guid(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C),
            pid = 2
        };
        public static _tagpropertykey WPD_OBJECT_NAME = new _tagpropertykey()
        {
            fmtid = new Guid(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C),
            pid = 4
        };

        public static _tagpropertykey WPD_PROPERTY_COMMON_HRESULT = new _tagpropertykey()
        {
            fmtid = new Guid(0xF0422A9C, 0x5DC8, 0x4440, 0xB5, 0xBD, 0x5D, 0xF2, 0x88, 0x35, 0x65, 0x8A),
            pid = 1003
        };
    }
}
