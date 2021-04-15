<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ms="urn:schemas-microsoft-com:xslt" xmlns:dt="urn:schemas-microsoft-com:datatypes">
  <xsl:output method="html" indent="yes"/>
  <xsl:template match="Root">
    <body style="font-family: Arial, sans-serif;font-size: 16px;line-height: 24px;">
      <p>
        Dear <xsl:value-of select="Name" />,
      </p>

      <p>
        You have made a provisional booking to visit the reading rooms at The National Archives on <xsl:value-of select="VisitStartDate" />.
      </p>

      <p>
        To confirm your booking, tell us which documents you want to view during your visit. If you haven’t already done so, follow this link: <xsl:value-of select="ReturnURL" />.
        You will need your booking reference and reader's ticket number to complete this step.
      </p>

      <p>
        If you do not complete this step by <xsl:value-of select="CompleteByDate" /> British Summer Time (BST), your <b>provisional booking will be automatically cancelled.</b>
      </p>

      <p>You can ignore this step if you have already completed your document order but you can return and edit your order as many times as you’d like before the deadline above.</p>

      <h3 style="margin-top: 2em;">Your booking summary </h3>
      <p>
          Your booking reference is: <xsl:value-of select="BookingReference" /><br/>
          Your Reader’s ticket number is: <xsl:value-of select="ReaderTicket" />
      </p>
      <table style="text-align: left;border: 1px solid #999;border-collapse: collapse;margin: 2em 0;">
        <tbody>
          <tr>
            <th style="padding: 1em;border: 1px solid #999;width: 150px;vertical-align: top;">Visit type</th>
            <td style="padding: 1em;border: 1px solid #999;vertical-align: top;">
              <xsl:value-of select="VisitType" />
            </td>
          </tr>
          <tr>
            <th style="padding: 1em;border: 1px solid #999;width: 150px;vertical-align: top;">Visit date</th>
            <td style="padding: 1em;border: 1px solid #999;vertical-align: top;">
              <xsl:value-of select="VisitStartDate" />
            </td>
          </tr>
          <xsl:if test="string(ReadingRoom)">
            <tr>
              <th style="padding: 1em;border: 1px solid #999;width: 150px;vertical-align: top;">Reading room</th>
              <td style="padding: 1em;border: 1px solid #999;vertical-align: top;">
                <xsl:value-of select="ReadingRoom" />
              </td>
            </tr>
          </xsl:if>
          <tr>
            <th style="padding: 1em;border: 1px solid #999;width: 150px;vertical-align: top;">Seat number</th>
            <td style="padding: 1em;border: 1px solid #999;vertical-align: top;">
              <xsl:value-of select="SeatNumber" />
              <br />
              Based on availability, we may need to change your seat on the date of your visit.
            </td>
          </tr>
        </tbody>
      </table>

      <p>Use Discovery, our catalogue, to gather your booking references: 
	    <a href="http://discovery.nationalarchives.gov.uk/ ">
			http://discovery.nationalarchives.gov.uk/
		</a>
	  </p>
      <h3 style="margin-top: 2em;">Cancel your visit</h3>
      <p>
        You can cancel your visit at any time. Use this link: <xsl:value-of select="ReturnURL" />
      </p>

      <h3 style="margin-top: 2em;">Need help?</h3>
      <p>
		  This email inbox is not being monitored. Contact us if you need help with this service:
		  <a href="https://www.nationalarchives.gov.uk/contact-us/">
			  https://www.nationalarchives.gov.uk/contact-us/
		  </a>
	  </p>
    </body>
  </xsl:template>
</xsl:stylesheet>
