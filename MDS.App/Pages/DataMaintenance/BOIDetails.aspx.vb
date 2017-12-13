Imports Oracle.ManagedDataAccess.Client

Public Class BOIDetails
    Inherits System.Web.UI.Page

    'Oracle
    Private oOra As New Oracle
    Private cnnOra As OracleConnection = Nothing
    Private cnnOraString As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            GetBOINumber()
            GetGoodTypeList()
            GetInspectionList()

            'Get id from querystring
            Dim invNo = Request.QueryString("INVNO")
            Dim invItem = Request.QueryString("INVITEM")

            GetDetails(invNo, invItem)

        End If

    End Sub

#Region "get App.Config"
    Private Sub GetAppConfig()
        cnnOraString = ConfigurationManager.ConnectionStrings("ORA_EQBConnString").ConnectionString
    End Sub
#End Region

#Region "Open Connection"
    Private Sub OpenConnection()
        oOra.OpenOraConnection(cnnOra, cnnOraString)
    End Sub

    Private Sub CloseConnection()
        oOra.CloseOraConnection(cnnOra)
    End Sub

#End Region

#Region "Functions"

    Private Sub GetBOINumber()

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT BOI_NUMBER FROM TBL_BOINUMBER ORDER BY BOI_NUMBER"

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                ddlBOINo.DataSource = dsResult.Tables(0)
                ddlBOINo.DataBind()

            End If


        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

    Private Sub GetGoodTypeList()

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT GOOD_TYPE_CODE, GOOD_TYPE_DESC FROM tbl_boigoodtypescode ORDER BY GOOD_TYPE_CODE"

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            ddlGoodType.DataSource = dsResult
            ddlGoodType.DataBind()


        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

    Private Sub GetInspectionList()

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT '0' AS INSPECTION_CODE, '--SELECT ONE--' AS INSPECTION_DESC, 'N' AS REQUIRE_ADDITIONAL_INFO FROM dual UNION SELECT INSPECTION_CODE, INSPECTION_DESC, REQUIRE_ADDITIONAL_INFO FROM TBL_BOIINSPECTIONCODE ORDER BY INSPECTION_CODE"

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            ddlInspectionType.DataSource = dsResult
            ddlInspectionType.DataBind()


        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

    Private Sub GetRequiredInspection(inspectionCode As String)

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "select REQUIRE_ADDITIONAL_INFO from tbl_boiinspectioncode WHERE INSPECTION_CODE = '" & inspectionCode & "'"

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                Dim flag = dsResult.Tables(0).Rows(0)("REQUIRE_ADDITIONAL_INFO").ToString()

                If flag.Trim = "Y" Then
                    ddlInspectionType.Enabled = False
                    pnlInspection.Visible = True
                Else
                    pnlInspection.Visible = False
                End If

            End If

        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

    Private Sub GetDetails(invNo As String, invItem As String)

        Try

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "SELECT * FROM TBL_BOIINFO WHERE INVOICE_NUMBER = '" & invNo & "' AND INVOICE_ITEM = '" & invItem & "' "

            Dim dsResult As DataSet = oOra.OraExecuteQuery(sSQL, cnnOra)

            If dsResult.Tables.Count > 0 And dsResult.Tables(0).Rows.Count > 0 Then

                'Bind the data
                txtItemNo.Text = dsResult.Tables(0).Rows(0)("ITEM_NUMBER").ToString()
                txtXMLType.Text = dsResult.Tables(0).Rows(0)("XMLTYPE").ToString()
                txtDocNo.Text = dsResult.Tables(0).Rows(0)("DOCUMENT_NUMBER").ToString()
                txtDocDate.Text = dsResult.Tables(0).Rows(0)("DOCUMENT_DATE").ToString()
                'txtBOITaxRef.Text = dsResult.Tables(0).Rows(0)("BOI_TAX_REFERENCE").ToString()
                'txtTaxRef.Text = dsResult.Tables(0).Rows(0)("TAX_REFERENCE").ToString()
                'txtBranch.Text = dsResult.Tables(0).Rows(0)("BRANCH").ToString()
                'txtTotalItem.Text = dsResult.Tables(0).Rows(0)("TOTAL_ITEM").ToString()
                'txtUserID.Text = dsResult.Tables(0).Rows(0)("USER_ID").ToString()
                'txtDeclareLineNo.Text = dsResult.Tables(0).Rows(0)("DECLARATION_LINE_NUMBER").ToString()
                txtImportDeclareNo.Text = dsResult.Tables(0).Rows(0)("IMPORT_DECLARATION_NUMBER").ToString()
                txtInvoiceNo.Text = dsResult.Tables(0).Rows(0)("INVOICE_NUMBER").ToString()
                txtInvoiceDate.Text = dsResult.Tables(0).Rows(0)("INVOICE_DATE").ToString()
                txtInvoiceItem.Text = dsResult.Tables(0).Rows(0)("INVOICE_ITEM").ToString()
                txtDescription.Text = dsResult.Tables(0).Rows(0)("DESCRIPTION").ToString()
                ddlGoodType.SelectedValue = dsResult.Tables(0).Rows(0)("GOOD_TYPE_CODE").ToString()
                'txtExemptType.Text = dsResult.Tables(0).Rows(0)("EXEMPT_TYPE").ToString()
                txtPrivilegeType.Text = dsResult.Tables(0).Rows(0)("PRIVILEGE_TYPE").ToString()
                'txtPrivilegeCondition.Text = dsResult.Tables(0).Rows(0)("PRIVILEGE_CONDITION").ToString()
                'txtConditionDutyRate.Text = dsResult.Tables(0).Rows(0)("CONDITION_DUTY_RATE").ToString()
                'txtPercentExemptDuty.Text = dsResult.Tables(0).Rows(0)("PERCENT_EXEMPT_DUTY").ToString()
                'txtPercentExemptVAT.Text = dsResult.Tables(0).Rows(0)("PERCENT_EXEMPT_VAT").ToString()
                txtUnitCode.Text = dsResult.Tables(0).Rows(0)("UNIT_CODE").ToString()
                txtQuantity.Text = dsResult.Tables(0).Rows(0)("QUANTITY").ToString()
                'txtPrivilegeFrom.Text = dsResult.Tables(0).Rows(0)("PRIVILEGE_VALID_FROM").ToString()
                'txtPrivilegeUntil.Text = dsResult.Tables(0).Rows(0)("PRIVILEGE_VALID_UNTIL").ToString()
                'txtRefDocumentNo.Text = dsResult.Tables(0).Rows(0)("REFERENCE_DOCUMENT_NUMBER").ToString()
                ddlBOINo.SelectedValue = dsResult.Tables(0).Rows(0)("BOI_NUMBER").ToString()

                ddlInspectionType.SelectedValue = dsResult.Tables(0).Rows(0)("INSPECTION_CODE").ToString()

                txtCerNo.Text = dsResult.Tables(0).Rows(0)("CER_NUMBER").ToString()
                txtCerItem.Text = dsResult.Tables(0).Rows(0)("CER_ITEM").ToString()
                txtCerRemark.Text = dsResult.Tables(0).Rows(0)("CER_REMARK").ToString()

                If ddlInspectionType.SelectedItem.Text = "COMPLETED" Then

                    txtCerNo.ReadOnly = False
                    txtCerItem.ReadOnly = False
                    txtCerRemark.ReadOnly = False
                Else
                    txtCerNo.ReadOnly = True
                    txtCerItem.ReadOnly = True
                    txtCerRemark.ReadOnly = True
                End If

                GetRequiredInspection(dsResult.Tables(0).Rows(0)("INSPECTION_CODE").ToString())

            End If


        Catch ex As Exception

            Dim errorMsg As String = ex.Message

        Finally

            CloseConnection()

        End Try

    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Try

            If ddlInspectionType.SelectedItem.Text = "COMPLETED" Then

                If txtCerNo.Text.Trim = "" Or txtCerItem.Text.Trim = "" Or txtCerRemark.Text.Trim = "" Then

                    pnlError.Visible = True
                    lblError.Text = "ERROR: Cer No, Cer Item and Cer Remark MUST fill in for COMPLETED Inspection Type"
                Else
                    pnlError.Visible = False
                End If

            End If

            GetAppConfig()
            OpenConnection()

            Dim sSQL = "UPDATE TBL_BOIINFO SET "

            sSQL = sSQL & "BOI_NUMBER = '" & ddlBOINo.SelectedValue & "' "
            sSQL = sSQL & ", ITEM_NUMBER = '" & txtItemNo.Text & "' "
            sSQL = sSQL & ", XMLTYPE = '" & txtXMLType.Text & "' "
            sSQL = sSQL & ", DOCUMENT_NUMBER = '" & txtDocNo.Text & "' "
            sSQL = sSQL & ", DOCUMENT_DATE = TO_DATE('" & DateTime.Parse(txtDocDate.Text).ToString("yyyy-MM-dd") & "', 'yyyy-MM-dd') "
            'sSQL = sSQL & ", BOI_TAX_REFERENCE = '" & txtBOITaxRef.Text & "' "
            'sSQL = sSQL & ", TAX_REFERENCE = '" & txtTaxRef.Text & "' "
            'sSQL = sSQL & ", BRANCH = '" & txtBranch.Text & "' "
            'sSQL = sSQL & ", TOTAL_ITEM = '" & txtTotalItem.Text & "' "
            'sSQL = sSQL & ", USER_ID = '" & txtUserID.Text & "' "
            'sSQL = sSQL & ", DECLARATION_LINE_NUMBER = '" & txtDeclareLineNo.Text & "' "
            sSQL = sSQL & ", IMPORT_DECLARATION_NUMBER = '" & txtImportDeclareNo.Text & "' "
            'sSQL = sSQL & ", INVOICE_DATE = '" & txtInvoiceDate.Text & "' "
            sSQL = sSQL & ", DESCRIPTION = '" & txtDescription.Text & "' "
            'sSQL = sSQL & ", GOOD_TYPE_CODE = '" & ddlGoodType.SelectedValue.ToString & "' "
            'sSQL = sSQL & ", EXEMPT_TYPE = '" & txtExemptType.Text & "' "
            'sSQL = sSQL & ", PRIVILEGE_TYPE = '" & txtPrivilegeType.Text & "' "
            'sSQL = sSQL & ", PRIVILEGE_CONDITION = '" & txtPrivilegeCondition.Text & "' "
            'sSQL = sSQL & ", CONDITION_DUTY_RATE = '" & txtConditionDutyRate.Text & "' "
            'sSQL = sSQL & ", PERCENT_EXEMPT_DUTY = '" & txtPercentExemptDuty.Text & "' "
            'sSQL = sSQL & ", PERCENT_EXEMPT_VAT = '" & txtPercentExemptVAT.Text & "' "
            sSQL = sSQL & ", UNIT_CODE = '" & txtUnitCode.Text & "' "
            sSQL = sSQL & ", QUANTITY = '" & txtQuantity.Text & "' "
            'sSQL = sSQL & ", PRIVILEGE_VALID_FROM = TO_DATE('" & DateTime.Parse(txtPrivilegeFrom.Text).ToString("yyyy-MM-dd") & "', 'yyyy-MM-dd') "
            'sSQL = sSQL & ", PRIVILEGE_VALID_UNTIL = TO_DATE('" & DateTime.Parse(txtPrivilegeUntil.Text).ToString("yyyy-MM-dd") & "', 'yyyy-MM-dd') "
            'sSQL = sSQL & ", REFERENCE_DOCUMENT_NUMBER = '" & txtRefDocumentNo.Text & "' "
            sSQL = sSQL & ", INSPECTION_CODE = '" & ddlInspectionType.SelectedValue.ToString() & "' "
            sSQL = sSQL & ", CER_NUMBER = '" & txtCerNo.Text & "' "
            sSQL = sSQL & ", CER_ITEM = '" & txtCerItem.Text & "' "
            sSQL = sSQL & ", CER_REMARK = '" & txtCerRemark.Text & "' "

            sSQL = sSQL & "WHERE INVOICE_NUMBER = '" & txtInvoiceNo.Text & "' AND INVOICE_ITEM = '" & txtInvoiceItem.Text & "'"

            Dim dsResult As OracleCommand = oOra.OraExecuteInsertUpdate(sSQL, cnnOra)

            Response.Redirect("~/Pages/DataMaintenance/BOIList.aspx")

        Catch ex As Exception

            Dim errorMsg As String = ex.Message
            pnlError.Visible = True
            lblError.Text = errorMsg

        Finally

            CloseConnection()

        End Try

    End Sub

    Protected Sub ddlInspectionType_SelectedIndexChanged(sender As Object, e As EventArgs)

        If ddlInspectionType.SelectedItem.Text = "COMPLETED" Then

            txtCerNo.ReadOnly = False
            txtCerItem.ReadOnly = False
            txtCerRemark.ReadOnly = False
        Else
            txtCerNo.ReadOnly = True
            txtCerItem.ReadOnly = True
            txtCerRemark.ReadOnly = True
        End If

    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        Try

            Dim sSQL As String = ""

            GetAppConfig()
            OpenConnection()

            sSQL = "DELETE FROM TBL_BOIINFO WHERE INVOICE_NUMBER = '" & txtInvoiceNo.Text & "' AND INVOICE_ITEM = '" & txtInvoiceItem.Text & "'"

            Dim oc As OracleCommand = oOra.OraExecuteInsertUpdate(sSQL, cnnOra)
            CloseConnection()

            Response.Redirect("~/Pages/DataMaintenance/BOIList.aspx")

        Catch ex As Exception

            Dim errorMsg As String = ex.Message
            pnlError.Visible = True
            lblError.Text = errorMsg

        Finally

            CloseConnection()

        End Try

    End Sub

#End Region


End Class