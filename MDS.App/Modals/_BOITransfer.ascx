<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="_BOITransfer.ascx.vb" Inherits="MDS.App._BOITransfer" %>

<!-- Modal -->
<div id="modalBOITransfer" class="modal fade" role="dialog">
    <div class="modal-dialog modal-sm">

        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header" style="background-color: #b91717; color: #fff;">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">BOI Transfer</h4>
            </div>
            <div class="modal-body">

                <div class="row">

                    <div class="col-lg-4 col-md-4">
                        BOI Transfer Date: 
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <input class="form-control datepicker" id="modalTxtBOIDate" type="text" maxlength="50" data-date-format="yyyy-mm-dd">
                    </div>

                </div>

                <div class="row" style="margin-top:15px;">

                    <div class="col-lg-4 col-md-4">
                        BOI Number: 
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <input class="form-control" id="modalTxtBOINumber" type="text" maxlength="50">
                    </div>

                </div>

                <div class="row">

                    <div class="col-md-12">
                        <br />
                        <table id="modalList" class="table table-bordered">
                            <thead>
                                <tr style="background-color:#343434; color:#ffffff; font-weight:bold;">
                                    <th>EQUIPMENT ID</th>
                                    <th>BOI NUMBER</th>
                                    <th>INVOICE NUMBER</th>
                                    <th>INVOICE ITEM</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>

                    </div>

                </div>

            </div>
            <div class="modal-footer">

                <div class="col-md-12">

                    <div style="float: right;">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        &nbsp;
                        <button type="button" id="modalBtnTransfer" class="btn btn-primary">Transfer</button>
                    </div>

                </div>

            </div>
        </div>

    </div>
</div>
