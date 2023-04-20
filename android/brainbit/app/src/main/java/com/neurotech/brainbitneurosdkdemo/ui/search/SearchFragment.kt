package com.neurotech.brainbitneurosdkdemo.ui.search

import android.os.Bundle
import android.os.Handler
import android.os.Looper
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.databinding.DataBindingUtil
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.neurotech.brainbitneurosdkdemo.R
import com.neurotech.brainbitneurosdkdemo.databinding.SearchFragmentBinding
import com.neurotech.brainbitneurosdkdemo.neuroimpl.BrainBitController
import com.neurosdk2.helpers.PermissionHelper
import com.neurosdk2.neuro.types.SensorState

class SearchFragment : Fragment() {

    companion object {
        fun newInstance() = SearchFragment()
    }

    private lateinit var binding: SearchFragmentBinding
    private lateinit var viewModel: SearchViewModel

    private var isSearch = false

    private val devicesListAdapter = DevicesListAdapter( mutableListOf())

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        binding = DataBindingUtil.inflate(inflater, R.layout.search_fragment, container, false)

        viewModel = ViewModelProvider(this)[SearchViewModel::class.java]
        binding.viewModel = viewModel

        val llm = LinearLayoutManager(context)
        llm.orientation = LinearLayoutManager.VERTICAL
        binding.searchDevicesList.layoutManager = llm
        binding.searchDevicesList.adapter = devicesListAdapter
        binding.searchDevicesList.setOnItemClickListener {
            val diviceNumber = it

            devicesListAdapter.devices[diviceNumber].inProgress = true
            devicesListAdapter.notifyItemChanged(diviceNumber)

                BrainBitController.createAndConnect(devicesListAdapter.devices[diviceNumber].sInfo, onConnectionResult = {
                    Handler(Looper.getMainLooper()).post {
                        if(it == SensorState.StateInRange){
                            findNavController().popBackStack(R.id.menuFragment, false)
                        }
                        else {
                            Toast.makeText(requireContext(), "Device connection fail!", Toast.LENGTH_SHORT).show()
                        }

                        devicesListAdapter.devices[diviceNumber].inProgress = false
                        devicesListAdapter.notifyItemChanged(diviceNumber)
                    }
                })
        }

        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        binding.searchButton.setOnClickListener {
            if(!PermissionHelper.HasAllPermissions(requireContext())){
                PermissionHelper.RequestPermissions(requireContext()
                ) { grantedPermissions, deniedPermissions, deniedPermanentlyPermissions -> viewModel.onSearchClicked() }
            }
            else
            {
               viewModel.onSearchClicked()
            }

            isSearch = !isSearch
        }

        viewModel.sensors.observe(viewLifecycleOwner){
            devicesListAdapter.devices.clear()
            for(device in it){
                devicesListAdapter.devices.add(DeviceListItem(device.name, device.address, false, device))
            }
            devicesListAdapter.notifyDataSetChanged()
        }

    }

    override fun onDestroyView() {
        super.onDestroyView()

        if (isSearch)
            viewModel.close()
    }


    private inline fun RecyclerView.setOnItemClickListener(crossinline listener: (position: Int) -> Unit) {
        addOnItemTouchListener(RecyclerItemClickListener(this,
            object : RecyclerItemClickListener.OnItemClickListener {
                override fun onItemClick(view: View, position: Int) {
                    listener(position)
                }
            }))
    }


}