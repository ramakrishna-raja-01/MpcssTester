using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MPCLib.Constants;
using MPCLib.Dtos.MPCSS;
using MPCLib.Dtos.Request;
using MPCLib.Dtos.Response;
using MPCLib.ResponseHandler;
using MPCLib.Services;
using MPCLib.Services.interfaces;
using Newtonsoft.Json;
using Serilog;

namespace QueueSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        //private readonly ILogger _logger;
        private readonly IMpcssService _mpcssService;

        public WeatherForecastController(IMpcssService mpcssService)
        {
           // _logger = logger;
            _mpcssService = mpcssService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpPost("mpcssRegistrationSendSsl/{id}")]
        public object MpcssRegistrationSendssl(MpcssMessageType id, RegistrationRecordInputDto request)
        {
            try
            {
                Console.WriteLine("MQURL: " + request.ActivemqUrl);
                System.Diagnostics.Debug.WriteLine("MQURL: " + request.ActivemqUrl);

                _mpcssService.SendRecordRegistrationRequest(request, id);
                return "message sent";
            }catch(Exception ex)
            {
                Console.WriteLine("ERROR: " + ex);
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex);
                return "message failed";
            }
        }

        [HttpPost("mpcssAccountRegistrationSend/{id}")]
        public object MpcssAccountRegistrationSend(MpcssMessageType id, RegistrationAccountInputDto request)
        {
            _mpcssService.SendAccountRegistrationRequestAsync(request, id);
            return "message sent";
        }

        [HttpPost("mpcssCreditSend/{id}")]
        public object MpcssCreditSend(MpcssMessageType id, CreditDebitPaymentInputDto request)
        {
            _mpcssService.SendPaymentCreditDebitRequestAsync(request, id);
            return "message sent";
        }

        [HttpPost("mpcssDebitSend/{id}")]
        public object MpcssDebitSend(MpcssMessageType id, CreditDebitPaymentInputDto request)
        {
            _mpcssService.SendPaymentCreditDebitRequestAsync(request, id);
            return "message sent";
        }

        [HttpPost("mpcssEnquirySend/{id}")]
        public object MpcssEnquirySend(MpcssMessageType id, PaymentEnquiryInputDto request)
        {
            _mpcssService.SendPaymentEnquiryRequestAsync(request, id);
            return "message sent";
        }

        [HttpPost("mpcssStatusReportSend/{id}")]
        public object MpcssStatusReportSend(MpcssMessageType id, PaymentStatusReportInputDto request)
        {
            _mpcssService.SendPaymentStatusReportRequestAsync(request, id);
            return "message sent";
        }

        [HttpPost("mpcssReturnRequest/{id}")]
        public object SendPaymentReturnRequestAsync(MpcssMessageType id, ReturnRequestInputDto request)
        {
            _mpcssService.SendPaymentReturnRequestAsync(request, id);
            return "message sent";
        }

        [HttpPost("nameVerification/{id}")]
        public object SendCustomerNameVerificationRequestAsync(MpcssMessageType id, CustomerNameVerificationInputDto request)
        {
             _mpcssService.SendCustomerNameVerificationRequestAsync(request, id);
            return "message sent";
        }

        [HttpPost("defaultAccount/{id}")]
        public object SendDefaultAccountVerificationRequestAsync(MpcssMessageType id, DefaultAccountInputDto request)
        {
            _mpcssService.SendDefaultAccountVerificationRequestAsync(request, id);
            return "message sent";
        }

        [HttpPost("startListnr")]
        public ServiceResponse StartListnr()
        {
            
            Log.Information($"Inside StartListnr");
            var response = _mpcssService.StartActiveMqListeners();
            Log.Information($"after StartActiveMqListeners");
            return response;
        }

        [HttpPost("regResponse")]
        public object StartListnrtets([FromBody] string response)
        {
            Log.Information("Response From MPCSS Library: " + response);
            RedirectResponse<RegistrationResponse> myDeserializedClass = JsonConvert.DeserializeObject<RedirectResponse<RegistrationResponse>>(response);
            RegistrationResponse regResponse = myDeserializedClass.Result;
            return regResponse;
        }

        [HttpPost("customerNameResponse")]
        public object CustomerNameResponse([FromBody] string response)
        {
            Log.Information("Response From MPCSS Library: " + response);
            RedirectResponse<CustomerNameVerificationResponse> myDeserializedClass = JsonConvert.DeserializeObject<RedirectResponse<CustomerNameVerificationResponse>>(response);
            CustomerNameVerificationResponse serializedResponse = myDeserializedClass.Result;
            return serializedResponse;
        }

        [HttpPost("defaultAccountResponse")]
        public object DefaultAccountResponse([FromBody] string response)
        {
            Log.Information("Response From MPCSS Library: " + response);
            RedirectResponse<DefaultAccountVerificationResponse> myDeserializedClass = JsonConvert.DeserializeObject<RedirectResponse<DefaultAccountVerificationResponse>>(response);
            DefaultAccountVerificationResponse serializedResponse = myDeserializedClass.Result;
            return serializedResponse;
        }

        [HttpPost("creditResponse")]
        public object SendCreditPaymentResponse([FromBody] string response)
        {
            Console.WriteLine("Response From MPCSS Library: " + response);
            System.Diagnostics.Debug.WriteLine("Response From MPCSS Library: " + response);
            DateTime dateTime = DateTime.Now;
            string MessageId = "ABCD" + dateTime.Year + dateTime.Month + dateTime.Day  + dateTime.Minute + dateTime.Second ;
            if (response != null)
            {
                RedirectResponse<CreditTransferResponseDto> myDeserializedClass = JsonConvert.DeserializeObject<RedirectResponse<CreditTransferResponseDto>>(response);
                CreditTransferResponseDto creditTransferResponse = myDeserializedClass.Result;
                if (creditTransferResponse != null)
                {
                    PaymentStatusReportInputDto paymentStatusReport = new PaymentStatusReportInputDto
                    {
                        MessageIdentificationCode = MessageId,
                        OriginalMessageId = creditTransferResponse.GrpHdr.PaymentMessageId,
                        OriginalMessageNameId = MpcssMessageConstants.CREDIT_MESSAGE_TYPE,
                        GroupStatus = "ACSP",
                        BatchSource = creditTransferResponse.SplmtryData.Envlp.achSupplementaryData.BatchSource,
                        InstructingAgentBICFI = MpcssMessageConstants.ParticipantId,
                        InstructedAgentBICFI = creditTransferResponse.CdtTrfTxInf.DbtrAgt.FinInstnId.BICFI
                    };
                    var result = _mpcssService.SendPaymentStatusReportRequestAsync(paymentStatusReport, MpcssMessageType.PaymentStatusReport);
                    return result;
                }
                
            }

            return null;
        }

        [HttpPost("debitResponse")]
        public object SendDebitPaymentResponse([FromBody] string response)
        {
            Console.WriteLine("Response From MPCSS Library: " + response);
            System.Diagnostics.Debug.WriteLine("Response From MPCSS Library: " + response);
            DateTime dateTime = DateTime.Now;
            string MessageId = "ABCD" + dateTime.Year + dateTime.Month + dateTime.Day + dateTime.Minute + dateTime.Second;
            if (response != null )
            {
                RedirectResponse<DirectDebitTransferResponseDto> myDeserializedClass = JsonConvert.DeserializeObject<RedirectResponse<DirectDebitTransferResponseDto>>(response);
                DirectDebitTransferResponseDto debitTransferResponseDto = myDeserializedClass.Result;
                if (debitTransferResponseDto != null)
                {
                    PaymentStatusReportInputDto paymentStatusReport = new PaymentStatusReportInputDto
                    {
                        MessageIdentificationCode = MessageId,
                        OriginalMessageId = debitTransferResponseDto.GrpHdr.PaymentMessageId,
                        OriginalMessageNameId = MpcssMessageConstants.DEBIT_MESSAGE_TYPE,
                        GroupStatus = "ACSP",
                        BatchSource = debitTransferResponseDto.SplmtryData.Envlp.achSupplementaryData.BatchSource,
                        InstructingAgentBICFI = MpcssMessageConstants.ParticipantId,
                        InstructedAgentBICFI = debitTransferResponseDto.DrctDbtTxInf.CdtrAgt.FinInstnId.BICFI

                    };
                    var result = _mpcssService.SendPaymentStatusReportRequestAsync(paymentStatusReport, MpcssMessageType.PaymentStatusReport);
                    return result;
                }
                
            }
            
            return response;
        }

        [HttpPost("returnResponse")]
        public object SendPaymentReturnResponse([FromBody] string response)
        {
            Console.WriteLine("Response From MPCSS Library: " + response);
            System.Diagnostics.Debug.WriteLine("Response From MPCSS Library: " + response);
             DateTime dateTime = DateTime.Now;
             string MessageId = "ABCD" + dateTime.Year + dateTime.Month + dateTime.Day + dateTime.Minute + dateTime.Second;
             if (response != null)
             {
                RedirectResponse<PaymentReturnResponseDto> myDeserializedClass = JsonConvert.DeserializeObject<RedirectResponse<PaymentReturnResponseDto>>(response);
                PaymentReturnResponseDto paymentReturn = myDeserializedClass.Result;
                if (paymentReturn != null)
                {
                    PaymentStatusReportInputDto paymentStatusReport = new PaymentStatusReportInputDto
                    {
                        MessageIdentificationCode = MessageId,
                        OriginalMessageId = paymentReturn.GrpHdr.PaymentMessageId,
                        OriginalMessageNameId = MpcssMessageConstants.PAYMENT_RETURN_MESSAGE_TYPE,
                        GroupStatus = "ACSP",
                        BatchSource = paymentReturn.SplmtryData.Envlp.achSupplementaryData.BatchSource,
                        InstructingAgentBICFI = MpcssMessageConstants.ParticipantId,
                        InstructedAgentBICFI = MpcssMessageConstants.ParticipantId

                    };
                    var result = _mpcssService.SendPaymentStatusReportRequestAsync(paymentStatusReport, MpcssMessageType.PaymentStatusReport);
                    return result;
                }
                
             }
             
            return response;
        }
        
        [HttpPost("enquiryResponse")]
        public object SendPaymentEnquiryResponse([FromBody] string response)
        {
            Console.WriteLine("Response From MPCSS Library: " + response);
            System.Diagnostics.Debug.WriteLine("Response From MPCSS Library: " + response);
            RedirectResponse<PaymentEnquiryResponseDto> myDeserializedClass = JsonConvert.DeserializeObject<RedirectResponse<PaymentEnquiryResponseDto>>(response);
            PaymentEnquiryResponseDto paymentEnquiry = myDeserializedClass.Result;
            return paymentEnquiry;
        }

        [HttpPost("paymentResponse")]
        public object SendPaymentStatusResponse([FromBody] string response)
        {
            Console.WriteLine("Response From MPCSS Library: " + response);
            System.Diagnostics.Debug.WriteLine("Response From MPCSS Library: " + response);
            DateTime dateTime = DateTime.Now;
            RedirectResponse<PaymentReportResponse> myDeserializedClass = JsonConvert.DeserializeObject<RedirectResponse<PaymentReportResponse>>(response);
            PaymentReportResponse resp = myDeserializedClass.Result;
            return resp;
        }
    }
}
