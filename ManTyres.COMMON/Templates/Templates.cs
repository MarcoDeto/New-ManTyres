using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManTyres.COMMON.Templates
{
	public static class Templates
	{
		// TEMPORANEO, DA ELIMINARE
		public static string ClaimStatusOpen() => @"<div>
   <div style='font-family:Roboto, RobotoDraft, Helvetica, Arial, sans-serif;font-size:14px;width:100%;background:#fafafa;padding:40px 0 30px;'>
      <div style='width:600px;max-width:100%;background:#fff;border-radius:6px;margin:0 auto;'>
         <!-- <img width='100%' src='https://mgcrmbotaee8.blob.core.windows.net/teq-link/logo.png' style='background:#000;'> -->
         <div style='padding:16px 40px 40px;'>
            <h3 style='font-weight:bold;font-size:24px;line-height:36px;margin:16px 0;color:#1e2026;'>
               A new claim has been opened.
            </h3>
            <h3 style='font-weight:bold;font-size:20px;line-height:36px;margin:16px 0;color:#1e2026;'>
               Claim n.__CLAIMNUMBER__
            </h3>
            <div style='margin:0;color:#474d57;font-size:16px;'>
               <p style='margin-bottom:26px;'>
                  Invoice reference: __INVOICENUMBER__.
               </p>
            </div>
            <div style='padding:16px;margin:12px 0 32px;background-color:#fafafa;border-radius:4px;'>
               <table class='yiv4202240446deviceInfo'
                  style='width:100%;background:#fafafa;vertical-align:middle;padding:10px 0 10px;'>
                  <tbody>
                     <tr>
                        <td>
                           <table>
                              <tbody>
                                 <tr>
                                    <td style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Customer: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__CONTACTPERSON__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Email: </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__CONTACTEMAIL__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Phone: </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__CONTACTPHONE__</div>
                                       </div>
                                    </td>
                                 </tr>
                              </tbody>
                           </table>
                        </td>
                     </tr>
                  </tbody>
               </table>
            </div>
            <div style='padding:16px;margin:12px 0 32px;background-color:#fafafa;border-radius:4px;'>
               <table class='yiv4202240446deviceInfo'
                  style='width:100%;background:#fafafa;vertical-align:middle;padding:10px 0 10px;'>
                  <tbody>
                     <tr>
                        <td>
                           <table>
                              <tbody>
                                 <tr>
                                    <td style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Equipment type: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__EQUIPMENTFAMILY__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Installation date: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__INSTALLATIONDATE__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Failure date: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__FAILUREDATE__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Defect: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__DEFECT__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Description: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;word-break: normal;'><p>__DEFECTDESCRIPTION__</p></div>
                                       </div>
                                    </td>
                                 </tr>
                              </tbody>
                           </table>
                        </td>
                     </tr>
                  </tbody>
               </table>
            </div>
            <div>
               <p class='yiv4202240446section' style='font-weight:normal;margin:16px 0 0 0;color:#474d57;'>
                  Claim status: __CLAIMSTATUS__
               </p>
            </div>
            <div>
               <p class='yiv4202240446section' style='font-weight:normal;margin:16px 0 0 0;color:#474d57;'>
                  Operator: __EmailVsg__
               </p>
            </div>

            <div style='line-height:24px;color:#76808f;margin-top: 36px;'>
               Teq-Link Team
            </div>
         </div>
      </div>
      <div style='text-align:center;'>
         <p style='color:#aeb4bc !important;margin:0;'>
            <br>
            2021 © Teq-Link.com All Rights Reserved
            <br>
            URL：www.teq-link.com <br>
            E-mail：support@teq-link.com
         </p>
      </div>
   </div>
</div>";
		public static string ClaimStatusWorking() => @"<div>
   <div style='font-family:Roboto, RobotoDraft, Helvetica, Arial, sans-serif;font-size:14px;width:100%;background:#fafafa;padding:40px 0 30px;'>
      <div style='width:600px;max-width:100%;background:#fff;border-radius:6px;margin:0 auto;'>
         <!-- <img width='100%' src='https://mgcrmbotaee8.blob.core.windows.net/teq-link/logo.png' style='background:#000;'> -->
         <div style='padding:16px 40px 40px;'>
            <h3 style='font-weight:bold;font-size:24px;line-height:36px;margin:16px 0;color:#1e2026;'>
               Claim n.__CLAIMNUMBER__ status has been updated to __CLAIMSTATUS__.
            </h3>
            <div style='margin:0;color:#474d57;font-size:16px;'>
               <p style='margin-bottom:26px;'>
                  Invoice reference: __INVOICENUMBER__.
               </p>
            </div>
            <div style='padding:16px;margin:12px 0 32px;background-color:#fafafa;border-radius:4px;'>
               <table class='yiv4202240446deviceInfo'
                  style='width:100%;background:#fafafa;vertical-align:middle;padding:10px 0 10px;'>
                  <tbody>
                     <tr>
                        <td>
                           <table>
                              <tbody>
                                 <tr>
                                    <td style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Customer: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__CONTACTPERSON__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Email: </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__CONTACTEMAIL__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Phone: </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__CONTACTPHONE__</div>
                                       </div>
                                    </td>
                                 </tr>
                              </tbody>
                           </table>
                        </td>
                     </tr>
                  </tbody>
               </table>
            </div>
            <div style='padding:16px;margin:12px 0 32px;background-color:#fafafa;border-radius:4px;'>
               <table class='yiv4202240446deviceInfo'
                  style='width:100%;background:#fafafa;vertical-align:middle;padding:10px 0 10px;'>
                  <tbody>
                     <tr>
                        <td>
                           <table>
                              <tbody>
                                 <tr>
                                    <td style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Equipment type: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__EQUIPMENTFAMILY__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Installation date: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__INSTALLATIONDATE__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Failure date: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__FAILUREDATE__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Defect: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__DEFECT__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Description: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;word-break: normal;'><p>__DEFECTDESCRIPTION__</p></div>
                                       </div>
                                    </td>
                                 </tr>
                              </tbody>
                           </table>
                        </td>
                     </tr>
                  </tbody>
               </table>
            </div>
            <div>
               <p class='yiv4202240446section' style='font-weight:normal;margin:16px 0 0 0;color:#474d57;'>
                  Claim status: __CLAIMSTATUS__
               </p>
            </div>
            <div>
               <p class='yiv4202240446section' style='font-weight:normal;margin:16px 0 0 0;color:#474d57;'>
                  Operator: __EmailVsg__
               </p>
            </div>

            <div style='line-height:24px;color:#76808f;margin-top: 36px;'>
               Teq-Link Team
            </div>
         </div>
      </div>
      <div style='text-align:center;'>
         <p style='color:#aeb4bc !important;margin:0;'>
            <br>
            2021 © Teq-Link.com All Rights Reserved
            <br>
            URL：www.teq-link.com <br>
            E-mail：support@teq-link.com
         </p>
      </div>
   </div>
</div>";
		public static string ClaimStatusWorkingReqInfo() => @"
<div>
   <div style='font-family:Roboto, RobotoDraft, Helvetica, Arial, sans-serif;font-size:14px;width:100%;background:#fafafa;padding:40px 0 30px;'>
      <div style='width:600px;max-width:100%;background:#fff;border-radius:6px;margin:0 auto;'>
         <!-- <img width='100%' src='https://mgcrmbotaee8.blob.core.windows.net/teq-link/logo.png' style='background:#000;'> -->
         <div style='padding:16px 40px 40px;'>
            <h3 style='font-weight:bold;font-size:24px;line-height:36px;margin:16px 0;color:#1e2026;'>
               Claim n.__CLAIMNUMBER__ status has been updated. Additional information required.
            </h3>
            <div style='margin:0;color:#474d57;font-size:16px;'>
               <p style='margin-bottom:26px;'>
                  Invoice reference: __INVOICENUMBER__.
               </p>
            </div>
            <div style='padding:16px;margin:12px 0 32px;background-color:#fafafa;border-radius:4px;'>
               <table class='yiv4202240446deviceInfo'
                  style='width:100%;background:#fafafa;vertical-align:middle;padding:10px 0 10px;'>
                  <tbody>
                     <tr>
                        <td>
                           <table>
                              <tbody>
                                 <tr>
                                    <td style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Customer: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__CONTACTPERSON__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Email: </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__CONTACTEMAIL__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Phone: </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__CONTACTPHONE__</div>
                                       </div>
                                    </td>
                                 </tr>
                              </tbody>
                           </table>
                        </td>
                     </tr>
                  </tbody>
               </table>
            </div>
            <div style='padding:16px;margin:12px 0 32px;background-color:#fafafa;border-radius:4px;'>
               <table class='yiv4202240446deviceInfo'
                  style='width:100%;background:#fafafa;vertical-align:middle;padding:10px 0 10px;'>
                  <tbody>
                     <tr>
                        <td>
                           <table>
                              <tbody>
                                 <tr>
                                    <td style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Equipment type: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__EQUIPMENTFAMILY__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Installation date: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__INSTALLATIONDATE__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Failure date: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__FAILUREDATE__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Defect: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__DEFECT__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Description: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;word-break: normal;'><p>__DEFECTDESCRIPTION__</p></div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Required additional information: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;word-break: normal;'><p>__REQUIREDINFO__</p></div>
                                       </div>
                                    </td>
                                 </tr>
                              </tbody>
                           </table>
                        </td>
                     </tr>
                  </tbody>
               </table>
            </div>
            <div>
               <p class='yiv4202240446section' style='font-weight:normal;margin:16px 0 0 0;color:#474d57;'>
                  Claim status: __CLAIMSTATUS__
               </p>
            </div>
            <div>
               <p class='yiv4202240446section' style='font-weight:normal;margin:16px 0 0 0;color:#474d57;'>
                  Operator: __EmailVsg__
               </p>
            </div>

            <div style='line-height:24px;color:#76808f;margin-top: 36px;'>
               Teq-Link Team
            </div>
         </div>
      </div>
      <div style='text-align:center;'>
         <p style='color:#aeb4bc !important;margin:0;'>
            <br>
            2021 © Teq-Link.com All Rights Reserved
            <br>
            URL：www.teq-link.com <br>
            E-mail：support@teq-link.com
         </p>
      </div>
   </div>
</div>";
		public static string ClaimStatusClosedRefused() => @"<div>
   <div style='font-family:Roboto, RobotoDraft, Helvetica, Arial, sans-serif;font-size:14px;width:100%;background:#fafafa;padding:40px 0 30px;'>
      <div style='width:600px;max-width:100%;background:#fff;border-radius:6px;margin:0 auto;'>
         <!-- <img width='100%' src='https://mgcrmbotaee8.blob.core.windows.net/teq-link/logo.png' style='background:#000;'> -->
         <div style='padding:16px 40px 40px;'>
            <h3 style='font-weight:bold;font-size:24px;line-height:36px;margin:16px 0;color:#1e2026;'>
               Claim n.__CLAIMNUMBER__ has been refused.
            </h3>
            <div style='margin:0;color:#474d57;font-size:16px;'>
               <p style='margin-bottom:26px;'>
                  Invoice reference: __INVOICENUMBER__.
               </p>
            </div>
            <div style='padding:16px;margin:12px 0 32px;background-color:#fafafa;border-radius:4px;'>
               <table class='yiv4202240446deviceInfo'
                  style='width:100%;background:#fafafa;vertical-align:middle;padding:10px 0 10px;'>
                  <tbody>
                     <tr>
                        <td>
                           <table>
                              <tbody>
                                 <tr>
                                    <td style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Customer: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__CONTACTPERSON__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Email: </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__CONTACTEMAIL__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Phone: </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__CONTACTPHONE__</div>
                                       </div>
                                    </td>
                                 </tr>
                              </tbody>
                           </table>
                        </td>
                     </tr>
                  </tbody>
               </table>
            </div>
            <div style='padding:16px;margin:12px 0 32px;background-color:#fafafa;border-radius:4px;'>
               <table class='yiv4202240446deviceInfo'
                  style='width:100%;background:#fafafa;vertical-align:middle;padding:10px 0 10px;'>
                  <tbody>
                     <tr>
                        <td>
                           <table>
                              <tbody>
                                 <tr>
                                    <td style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Equipment type: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__EQUIPMENTFAMILY__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Installation date: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__INSTALLATIONDATE__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Failure date: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__FAILUREDATE__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Defect: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__DEFECT__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Description: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;word-break: normal;'><p>__DEFECTDESCRIPTION__</p></div>
                                       </div>
                                    </td>
                                 </tr>
                              </tbody>
                           </table>
                        </td>
                     </tr>
                  </tbody>
               </table>
            </div>
            <div>
               <p class='yiv4202240446section' style='font-weight:normal;margin:16px 0 0 0;color:#474d57;'>
                  Claim status: __CLAIMSTATUS__
               </p>
            </div>
            <div>
               <p class='yiv4202240446section' style='font-weight:normal;margin:16px 0 0 0;color:#474d57;'>
                  Reason of refusal:
               </p>
            </div>
            <div>
               <p class='yiv4202240446section' style='font-weight:normal;margin:16px 0 0 0;color:#474d57;word-break: normal;'>
                  Lorem Ipsum: After careful analysis, our team has come to the conclusion that equipment failure was not due to a factory defect. As so,this kind of machine damage is not covered by the current warranty. Customer can ask for intervetion for an additional cost.
               </p>
            </div>
            <div>
               <p class='yiv4202240446section' style='font-weight:normal;margin:16px 0 0 0;color:#474d57;'>
                  Operator: __EmailVsg__
               </p>
            </div>

            <div style='line-height:24px;color:#76808f;margin-top: 36px;'>
               Teq-Link Team
            </div>
         </div>
      </div>
      <div style='text-align:center;'>
         <p style='color:#aeb4bc !important;margin:0;'>
            <br>
            2021 © Teq-Link.com All Rights Reserved
            <br>
            URL：www.teq-link.com <br>
            E-mail：support@teq-link.com
         </p>
      </div>
   </div>
</div>";
		public static string ClaimStatusClosedApproved() => @"<div>
   <div style='font-family:Roboto, RobotoDraft, Helvetica, Arial, sans-serif;font-size:14px;width:100%;background:#fafafa;padding:40px 0 30px;'>
      <div style='width:600px;max-width:100%;background:#fff;border-radius:6px;margin:0 auto;'>
         <!-- <img width='100%' src='https://mgcrmbotaee8.blob.core.windows.net/teq-link/logo.png' style='background:#000;'> -->
         <div style='padding:16px 40px 40px;'>
            <h3 style='font-weight:bold;font-size:24px;line-height:36px;margin:16px 0;color:#1e2026;'>
               Claim n.__CLAIMNUMBER__ has been approved.
            </h3>
            <div style='margin:0;color:#474d57;font-size:16px;'>
               <p style='margin-bottom:26px;'>
                  Invoice reference: __INVOICENUMBER__.
               </p>
            </div>
            <div style='padding:16px;margin:12px 0 32px;background-color:#fafafa;border-radius:4px;'>
               <table class='yiv4202240446deviceInfo'
                  style='width:100%;background:#fafafa;vertical-align:middle;padding:10px 0 10px;'>
                  <tbody>
                     <tr>
                        <td>
                           <table>
                              <tbody>
                                 <tr>
                                    <td style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Customer: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__CONTACTPERSON__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Email: </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__CONTACTEMAIL__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Phone: </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__CONTACTPHONE__</div>
                                       </div>
                                    </td>
                                 </tr>
                              </tbody>
                           </table>
                        </td>
                     </tr>
                  </tbody>
               </table>
            </div>
            <div style='padding:16px;margin:12px 0 32px;background-color:#fafafa;border-radius:4px;'>
               <table class='yiv4202240446deviceInfo'
                  style='width:100%;background:#fafafa;vertical-align:middle;padding:10px 0 10px;'>
                  <tbody>
                     <tr>
                        <td>
                           <table>
                              <tbody>
                                 <tr>
                                    <td style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Equipment type: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__EQUIPMENTFAMILY__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Installation date: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__INSTALLATIONDATE__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Failure date: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__FAILUREDATE__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Defect: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;'>__DEFECT__</div>
                                       </div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td
                                       style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                       Description: 
                                    </td>
                                    <td>
                                       <div>
                                          <div style='color:#474d57;word-break: normal;'><p>__DEFECTDESCRIPTION__</p></div>
                                       </div>
                                    </td>
                                 </tr>
                              </tbody>
                           </table>
                        </td>
                     </tr>
                  </tbody>
               </table>
            </div>
            <div>
               <p class='yiv4202240446section' style='font-weight:normal;margin:16px 0 0 0;color:#474d57;'>
                  Claim status: __CLAIMSTATUS__
               </p>
            </div>
            <div>
               <p class='yiv4202240446section' style='font-weight:normal;margin:16px 0 0 0;color:#474d57;'>
                  Operator: __EmailVsg__
               </p>
            </div>

            <div style='line-height:24px;color:#76808f;margin-top: 36px;'>
               Teq-Link Team
            </div>
         </div>
      </div>
      <div style='text-align:center;'>
         <p style='color:#aeb4bc !important;margin:0;'>
            <br>
            2021 © Teq-Link.com All Rights Reserved
            <br>
            URL：www.teq-link.com <br>
            E-mail：support@teq-link.com
         </p>
      </div>
   </div>
</div>";
		public static string ResetPasswordSubject(string cultureInfo)
		{
			// TODO: Costruire il testo nella lingua corretta a seconda del linguaggio.
			return "TeqLink: your distributor account has been created!";
		}
		public static string ResetPasswordBody(string cultureInfo, string email, string password, string businessName, string personalizedText)
		{
			string body = @$"<div style='font-family:Roboto, RobotoDraft, Helvetica, Arial, sans-serif;font-size:14px;width:100%;background:#fafafa;padding:40px 0 30px;'>
   <div style='width:600px;max-width:100%;background:#fff;border-radius:6px;margin:0 auto;'>
      <img width='100%' src='https://mgcrmbotaee8.blob.core.windows.net/teq-link/logo.png' style='background:#000;'>
      <div style='padding:16px 40px 40px;'>
         <h3 style='font-weight:bold;font-size:24px;line-height:36px;margin:16px 0;color:#1e2026;'>
            Hello {businessName}, your distributor account has been created.
         </h3>
         <div style='padding:16px;margin:12px 0 32px;background-color:#fafafa;border-radius:4px;'>
            <p>{personalizedText}</p>
            <br>
            <p>Please use the credentials found down below for your first log-in. After password change, your journey on TeqLink Portal begins! Thanks for joining our family.</p>
            <table class='yiv4202240446deviceInfo' style='width:100%;background:#fafafa;vertical-align:middle;padding:10px 0 10px;'>
               <tbody>
                  <tr>
                     <td>
                        <table>
                           <tbody>
                              <tr>
                                 <td style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                    Username: 
                                 </td>
                                 <td>
                                    <div>
                                       <div style='color:#474d57;'>{email}</div>
                                    </div>
                                 </td>
                              </tr>
                              <tr>
                                 <td
                                    style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                    Password: </td>
                                 <td>
                                    <div>
                                       <div style='color:#474d57;'>{password}</div>
                                    </div>
                                 </td>
                              </tr>
                              <tr>
                                 <td
                                    style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                    Login: </td>
                                 <td>
                                    <div>
                                       <div style='color:#474d57;'><a href='https://teqlinkportal.azurewebsites.net'>TeqLinkPortal</a></div>
                                    </div>
                                 </td>
                              </tr>
                           </tbody>
                        </table>
                     </td>
                  </tr>
               </tbody>
            </table>
         </div>
         <div style = 'line-height:24px;color:#76808f;margin-top: 36px;'>
               Teq - Link Team
         </div>
      </div>
      <div style='text-align:center;'>
         <p style='color:#aeb4bc !important;margin:0;'>
         <br>
         2021 © <a href='https://teqlinkportal.azurewebsites.net'>TeqLinkPortal</a> All Rights Reserved
         <br>
         URL：<a href='https://teqlinkportal.azurewebsites.net'>TeqLinkPortal</a><br>
         E - mail：support @teq-link.com
         </p>
      </div>
   </div>
</div>";
			return body;
		}

	}
}
